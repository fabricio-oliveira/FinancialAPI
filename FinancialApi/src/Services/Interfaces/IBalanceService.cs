using System;
using System.Collections.Generic;
using FinancialApi.Models.Entity;

namespace FinancialApi.Services
{
    public interface IBalanceService
    {
        List<Balance> CashFlow(Account account);

    }

}
