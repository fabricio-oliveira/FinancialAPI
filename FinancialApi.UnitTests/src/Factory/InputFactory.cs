using System;
using FinancialApi.src.Models.Entity;

namespace FinancialApiUnitTests.Factory
{
    public class InputFactory
    {
        public static Input Build(Action<Input> pred = null)
        {
            var input = new Input(DateTime.Now, 100.00m,new CashFlow());

            pred?.Invoke(input);
            return input;

        }
    }
}
