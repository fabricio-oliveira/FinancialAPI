using System;
using System.Text;
using System.Threading.Tasks;
using FinancialApi.Queue;
using RabbitMQ.Client.Events;

namespace FinancialApi.workers
{
    public class ConsolidateCashFlow
    {
        private readonly PaymentQueue _paymentQueue;
        private readonly ReceiptQueue _receiptQueue;
        private readonly EntryQueue _entryQueue;

        public ConsolidateCashFlow(PaymentQueue paymentQueue,
                                   ReceiptQueue receiptQueue,
                                   EntryQueue entryQueue)
        {
            this._paymentQueue = paymentQueue;
            this._receiptQueue = receiptQueue;
            this._entryQueue = entryQueue;

            this._paymentQueue.SetConsumer(Consumer_Received);
        }


        //public class SampleJob : Job
        //{
        //    public SampleJob(TimeSpan interval, TimeSpan timeout)
        //        : base("Sample Job", interval, timeout)
        //    {
        //    }

        //    public override Task Execute()
        //    {
        //        return new Task(() => Thread.Sleep(3000));
        //    }
        //}


        async Task Consumer_Received(object sender, BasicDeliverEventArgs @event)
        {
            var message = Encoding.UTF8.GetString(@event.Body);

            await Task.Delay(250);

            Console.WriteLine($"End processing {message}");
        }
    }
}
