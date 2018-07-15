using System;
using System.Threading.Tasks;
using FinancialApi.Models.Entity;
using FinancialApi.Queue;
using FinancialApi.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client.Events;

namespace FinancialApi.workers
{
    public class ConsolidateEntryWorker
    {
        private readonly IPaymentQueue _paymentQueue;
        private readonly IPaymentService _paymentService;

        private readonly IErrorQueue _errorQueue;
        private readonly IReceiptQueue _receiptQueue;
        private readonly IReceiptService _receiptService;

        private const int MAX_RETRY = 3;

        public ConsolidateEntryWorker(IPaymentService paymentService, IPaymentQueue paymentQueue,
                                      IReceiptService receiptService, IReceiptQueue receiptQueue,
                                      IErrorQueue errorQueue)
        {
            this._paymentQueue = paymentQueue;
            this._paymentService = paymentService;
            this._paymentQueue.SetConsumer(WrapperPay);

            this._receiptQueue = receiptQueue;
            this._receiptService = receiptService;
            this._receiptQueue.SetConsumer(WrappeReceive);

            this._errorQueue = errorQueue;
        }

        public async Task WrapperPay(object sender, BasicDeliverEventArgs @event)
        {
            var body = System.Text.Encoding.UTF8.GetString(@event.Body);
            var entry = Utils.StringUtil.FromJson<Entry>(body);

            try
            {
                var result = _paymentService.Pay(entry);
                await Task.Delay(250);
                Console.WriteLine("teste", result);
            }
            catch (DbUpdateConcurrencyException)
            {
                this._paymentQueue.Enqueue(entry, 4000); //4 seconds
            }
            catch (Exception e)
            {
                if (entry.Attempts > MAX_RETRY)
                    this._errorQueue.Enqueue(entry);
                else
                {
                    entry.Errors = e.Message;
                    this._receiptQueue.Enqueue(entry, 3 * 60000); //3 minutes
                }
            }

        }


        public async Task WrappeReceive(object sender, BasicDeliverEventArgs @event)
        {
            var body = System.Text.Encoding.UTF8.GetString(@event.Body);
            var entry = Utils.StringUtil.FromJson<Entry>(body);

            try
            {
                var result = _receiptService.Receive(entry);
                await Task.Delay(250);
                Console.WriteLine("teste", result);
            }
            catch (DbUpdateConcurrencyException)
            {
                this._receiptQueue.Enqueue(entry, 4000); //4 seconds
            }
            catch (Exception e)
            {
                if (entry.Attempts > MAX_RETRY)
                    this._errorQueue.Enqueue(entry);
                else
                {
                    entry.Errors = e.Message;
                    this._receiptQueue.Enqueue(entry, 3 * 60000); //3 minutes
                }
            }
        }

    }
}
