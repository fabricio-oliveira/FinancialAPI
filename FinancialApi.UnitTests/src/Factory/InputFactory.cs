using System;
using FinancialApi.Models.Entity;

namespace FinancialApiUnitTests.Factory
{
    public class InputFactory
    {
        public static Input Build(Action<Input> pred = null)
        {
            var input = new Input(DateTime.Now, 100.00m,new Balance());

            pred?.Invoke(input);
            return input;

        }
    }
}
