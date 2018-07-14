using System;
using FinancialApi.Models.DTO;

namespace FinancialApiUnitTests.Factory
{
    public class EntryFactory
    {
        public static EntryDTO Build(Action<EntryDTO> pred = null)
        {
            var input = new EntryDTO(DateTime.Now, 100.00m);

            pred?.Invoke(input);
            return input;

        }
    }
}
