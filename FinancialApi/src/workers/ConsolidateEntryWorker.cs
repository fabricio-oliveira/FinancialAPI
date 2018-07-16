using System;
using System.Threading;
using System.Threading.Tasks;
using FinancialApi.Models.Entity;
using FinancialApi.Queue;
using FinancialApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace FinancialApi.workers
{
    public class ConsolidateEntryWorker
    {
        readonly IPaymentQueue _paymentQueue;
        readonly IPaymentService _paymentService;

        readonly IErrorQueue _errorQueue;
        readonly IReceiptQueue _receiptQueue;

        readonly IReceiptService _receiptService;

        readonly ILogger _logger;

        const int MAX_RETRY = 3;

        public ConsolidateEntryWorker(IPaymentService paymentService, IPaymentQueue paymentQueue,
                                      IReceiptService receiptService, IReceiptQueue receiptQueue,
                                      IErrorQueue errorQueue, ILogger logger)
        {
            this._paymentQueue = paymentQueue;
            this._paymentService = paymentService;
            this._paymentQueue.SetConsumer(WrapperPay);

            this._receiptQueue = receiptQueue;
            this._receiptService = receiptService;
            this._receiptQueue.SetConsumer(WrappeReceive);


            this._logger = logger;
            this._errorQueue = errorQueue;
        }


        //public async Task TestReceive()
        //{
        //    var body = _paymentQueue.Dequeue();


        //    if (body != null)
        //        await _paymentService.Pay(body);

        //}

        //BackgroundJob.Enqueue(
        public async Task WrapperPay(object _, BasicDeliverEventArgs @event)
        {
            var body = System.Text.Encoding.UTF8.GetString(@event.Body);
            var entry = JsonConvert.DeserializeObject<Entry>(body);

            try
            {
                var result = await _paymentService.Pay(entry);
                Thread.Sleep(10000);
                _logger.LogInformation("teste {0}", body);
            }
            catch (DbUpdateConcurrencyException)
            {
                _paymentQueue.Enqueue(entry, 4000); //4 seconds
            }
            catch (Exception e)
            {
                if (entry.Attempts > MAX_RETRY)
                    _errorQueue.Enqueue(entry);
                else
                {
                    entry.Errors = e.Message;
                    _receiptQueue.Enqueue(entry, 3 * 60000); //3 minutes
                }
            }

        }


        public async Task WrappeReceive(object _, BasicDeliverEventArgs @event)
        {
            var body = System.Text.Encoding.UTF8.GetString(@event.Body);
            var entry = JsonConvert.DeserializeObject<Entry>(body);

            try
            {
                var result = await _receiptService.Receive(entry);
                Thread.Sleep(10000);
                _logger.LogInformation("teste {0}", body);
            }
            catch (DbUpdateConcurrencyException)
            {
                _receiptQueue.Enqueue(entry, 4000); //4 seconds
            }
            catch (Exception e)
            {
                if (entry.Attempts > MAX_RETRY)
                    _errorQueue.Enqueue(entry);
                else
                {
                    entry.Errors = e.Message;
                    _receiptQueue.Enqueue(entry, 3 * 60000); //3 minutes
                }
            }
        }

    }
}
