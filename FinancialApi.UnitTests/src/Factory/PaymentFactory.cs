﻿using System;
using FinancialApi.Models.Entity;

namespace FinancialApiUnitTests.src.Factory
{
    public class PaymentFactory
    {
        public static Payment Build(Action<Payment> pred = null)
        {
            var payment = new Payment("Op Teste", "1234567-8", "0123-4", "corrente", "012.345.678-90",
                               100.00m, 0.03m, DateTime.Now);

            pred?.Invoke(payment);
            return payment;

        }
    }
}