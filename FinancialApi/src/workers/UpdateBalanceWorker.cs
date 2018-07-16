using System;
using FinancialApi.Queue;
using FinancialApi.Services;


namespace FinancialApi.workers
{
    public class UpdateBalanceWorker
    {

        readonly IBalanceService _balanceService;
        readonly IReceiptQueue _receiptQueue;

        public UpdateBalanceWorker(IBalanceService balanceService,
                                   IReceiptQueue receiptQueue)
        {
            _balanceService = balanceService;
            _receiptQueue = receiptQueue;
        }

        public void Excute()
        {
            var test = _receiptQueue.Dequeue();

            Console.WriteLine("Recorrent Thread {0}", test);

        }


    }
}
