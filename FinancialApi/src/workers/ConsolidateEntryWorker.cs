using System;
using System.Threading.Tasks;
using FinancialApi.Models.Entity;
using FinancialApi.Queue;
using FinancialApi.Services;
using Hangfire;
using RabbitMQ.Client.Events;

namespace FinancialApi.workers
{
    public class ConsolidateEntryWorker
    {
        private readonly PaymentQueue _paymentQueue;
        private readonly PaymentService _paymentService;

        private readonly ReceiptQueue _receiptQueue;
        private readonly ReceiptService _receiptService;

        public ConsolidateEntryWorker(PaymentService paymentService, PaymentQueue paymentQueue,
                                      ReceiptService receiptService, ReceiptQueue receiptQueue)
        {
            this._paymentQueue = paymentQueue;
            this._paymentService = paymentService;
            this._paymentQueue.SetConsumer(WrapperPay);

            this._receiptQueue = receiptQueue;
            this._receiptService = receiptService;
            this._receiptQueue.SetConsumer(WrappeReceive);
        }

        public async Task WrapperPay(object sender, BasicDeliverEventArgs @event)
        {
            var body = System.Text.Encoding.UTF8.GetString(@event.Body);
            var entry = Utils.StringUtil.FromJson<Entry>(body);
            var result = _paymentService.Pay(entry);
            await Task.Delay(250);
            Console.WriteLine("teste", result);
        }


        public async Task WrappeReceive(object sender, BasicDeliverEventArgs @event)
        {
            var body = System.Text.Encoding.UTF8.GetString(@event.Body);
            var entry = Utils.StringUtil.FromJson<Entry>(body);

            var result = _receiptService.Receive(entry);
            await Task.Delay(250);
            Console.WriteLine("teste", result);
        }

    }
}
