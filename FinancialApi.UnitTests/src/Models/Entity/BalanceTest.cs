using FinancialApiUnitTests.Factory;
using NUnit.Framework;

namespace FinancialApi.UnitTests.Models.Entity
{
    public class BalanceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [TestCase("10.00", "20.00", "1.00")]
        [TestCase("20.00", "10.00", "-0.50")]
        [TestCase("0.00", "0.00", "0.00")]
        [TestCase("0.00", "100.00", null)]
        [TestCase("100.0", "0.00", "-1.0")]
        [TestCase("1.00", "0.70", "-0.30")]
        public void UpdateDayPostionNewDayTest(decimal yestarday,
                                               decimal today,
                                               decimal? DayPositionToday)
        {
            //behavior
            var balance = BalanceFactory.Build(x => x.Total = today);


            //test
            balance.UpdateDayPostionNewDay(yestarday);

            //assert
            Assert.AreEqual(balance.DayPosition,DayPositionToday);
        }

        [TestCase("100.00", "10", "1.00","1.10")]
        public void UpdateDayPostionNewEntryTest(decimal total,
                                              decimal newEntry,
                                              decimal DayPositionAfter,
                                              decimal DayPositionBefore)
        {
            //behavior
            var balance = BalanceFactory.Build(x =>
            {
                x.Total = total;
                x.DayPosition = DayPositionAfter;
            });

            //test
            balance.UpdateDayPostionNewEntry(newEntry);

            //assert
            Assert.AreEqual(DayPositionBefore, balance.DayPosition);
        }

    }
}
