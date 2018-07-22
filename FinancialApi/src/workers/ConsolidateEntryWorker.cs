using System;
using FinancialApi.Models.Entity;
using FinancialApi.Queue;
using FinancialApi.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinancialApi.workers
{
    public class ConsolidateEntryWorker
    {
        readonly IPaymentQueue _paymentQueue;
        readonly IPaymentService _paymentService;

        readonly IErrorQueue _errorQueue;
        readonly IReceiptQueue _receiptQueue;

        readonly IReceiptService _receiptService;

        readonly ILogger<ConsolidateEntryWorker> _logger;

        const int MAX_RETRY = 3;
        const int DELAY_RETRAY_CONCURRENCY = 4000; // 4 SEG
        const int DELAY_RETRAY_OTHER_FAIL = 3 * 60000; // 3 * 60 MIN * SEG

        public ConsolidateEntryWorker(IPaymentService paymentService, IPaymentQueue paymentQueue,
                                      IReceiptService receiptService, IReceiptQueue receiptQueue,
                                      IErrorQueue errorQueue, ILogger<ConsolidateEntryWorker> logger)
        {
            _paymentQueue = paymentQueue;
            _paymentService = paymentService;

            _receiptQueue = receiptQueue;
            _receiptService = receiptService;

            _logger = logger;
            _errorQueue = errorQueue;
        }

        public void WorkManagerPay()
        {
            while (true)
            {
                var payment = _paymentQueue.Dequeue();
                if (payment != null) BackgroundJob.Enqueue(() => WrapperPay(payment));
            }
        }

        public void WorkManagerReceipt()
        {
            while (true)
            {
                var receipt = _receiptQueue.Dequeue();
                if (receipt != null) BackgroundJob.Enqueue(() => WrappeReceive(receipt));
            }

        }

        public void WrapperPay(Entry entry)
        {
            try
            {
                _logger.LogDebug("Process payment start {0}", entry.UUID);
                var result = _paymentService.Pay(entry);
                _logger.LogDebug("Process payment finish {0}", entry.UUID);
            }
            catch (DbUpdateConcurrencyException)
            {
                _paymentQueue.Enqueue(entry, DELAY_RETRAY_CONCURRENCY);
                _logger.LogDebug("Fail concurrency in process payment {0}, reeque msg", entry.UUID);
            }
            catch (Exception e)
            {
                if (entry.Attempts > MAX_RETRY)
                {
                    _errorQueue.Enqueue(entry);
                    _logger.LogDebug("Max Fail in process payment {0}, add error queue", entry.UUID);
                }
                else
                {
                    entry.Errors = e.Message;
                    _receiptQueue.Enqueue(entry, DELAY_RETRAY_OTHER_FAIL); //3 minutes
                    _logger.LogDebug("Unexpected Fail in process payment {0}, requeue Payment", entry.UUID);
                }
            }

        }

        public void WrappeReceive(Entry entry)
        {
            try
            {
                _logger.LogDebug("Process receipt start {0}", entry.UUID);
                var result = _receiptService.Receive(entry);
                _logger.LogDebug("Process receipt finish {0}", entry.UUID);
            }
            catch (DbUpdateConcurrencyException)
            {
                _receiptQueue.Enqueue(entry, DELAY_RETRAY_CONCURRENCY);
                _logger.LogDebug("Fail concurrency in process receipt {0}, reeque msg", entry.UUID);
            }
            catch (Exception e)
            {
                if (entry.Attempts > MAX_RETRY)
                {
                    _errorQueue.Enqueue(entry);
                    _logger.LogDebug("Max Fail in process receipt {0}, add error queue", entry.UUID);
                }
                else
                {
                    entry.Errors = e.Message;
                    _receiptQueue.Enqueue(entry, DELAY_RETRAY_OTHER_FAIL);
                    _logger.LogDebug("Unexpected Fail in process payment {0}, requeue Payment", entry.UUID);
                }
            }
        }

    }
}
