using System;
using System.Threading;
using System.Threading.Tasks;
using FinancialApi.Models.Entity;
using FinancialApi.Queue;
using FinancialApi.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RabbitMQ.Client.Events;

namespace FinancialApi.workers
{
    public class ConsolidateEntryWorker
    {
        readonly IPaymentQueue paymentQueue;
        readonly IPaymentService paymentService;

        readonly IErrorQueue errorQueue;
        readonly IReceiptQueue receiptQueue;
        readonly IReceiptService receiptService;

        const int MAX_RETRY = 3;

        public ConsolidateEntryWorker(IPaymentService paymentService, IPaymentQueue paymentQueue,
                                      IReceiptService receiptService, IReceiptQueue receiptQueue,
                                      IErrorQueue errorQueue)
        {
            this.paymentQueue = paymentQueue;
            this.paymentService = paymentService;
            this.paymentQueue.SetConsumer(WrapperPay);

            this.receiptQueue = receiptQueue;
            this.receiptService = receiptService;
            this.receiptQueue.SetConsumer(WrappeReceive);

            this.errorQueue = errorQueue;
        }


        public async Task TestReceive()
        {
            var body = paymentQueue.Dequeue();
            Console.WriteLine("teste {0}", body);
            if (body != null)
                await paymentService.Pay(body);

        }

        public async Task WrapperPay(object _, BasicDeliverEventArgs @event)
        {
            var body = System.Text.Encoding.UTF8.GetString(@event.Body);
            var entry = JsonConvert.DeserializeObject<Entry>(body);

            try
            {
                var result = await paymentService.Pay(entry);
                Thread.Sleep(10000);
                Console.WriteLine("teste {0}", result);
            }
            catch (DbUpdateConcurrencyException)
            {
                paymentQueue.Enqueue(entry, 4000); //4 seconds
            }
            catch (Exception e)
            {
                if (entry.Attempts > MAX_RETRY)
                    errorQueue.Enqueue(entry);
                else
                {
                    entry.Errors = e.Message;
                    receiptQueue.Enqueue(entry, 3 * 60000); //3 minutes
                }
            }

        }


        public async Task WrappeReceive(object _, BasicDeliverEventArgs @event)
        {
            var body = System.Text.Encoding.UTF8.GetString(@event.Body);
            var entry = JsonConvert.DeserializeObject<Entry>(body);

            try
            {
                var result = await receiptService.Receive(entry);
                Thread.Sleep(10000);
                Console.WriteLine("teste {0}", result);
            }
            catch (DbUpdateConcurrencyException)
            {
                receiptQueue.Enqueue(entry, 4000); //4 seconds
            }
            catch (Exception e)
            {
                if (entry.Attempts > MAX_RETRY)
                    errorQueue.Enqueue(entry);
                else
                {
                    entry.Errors = e.Message;
                    receiptQueue.Enqueue(entry, 3 * 60000); //3 minutes
                }
            }
        }

    }
}
