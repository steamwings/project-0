using Serilog;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Project0;

namespace UnitTestProject0
{
    [TestClass]
    public class UnitTestAccounts
    {

        [TestMethod]
        public void TestMonthsBefore()
        {
            UnitTesting.SetupTesting();
            int res = new DateTime(2019, 08, 30).MonthsBefore(new DateTime(2019, 09, 30));
            Log.Debug($"MonthsFrom result 1: {res}");
            Assert.AreEqual(1,res);
            res = new DateTime(2019, 08, 30).MonthsBefore(new DateTime(2019, 09, 29));
            Log.Debug($"MonthsFrom result 2: {res}");
            Assert.AreEqual(0, res);
            res = new DateTime(2019, 09, 30).MonthsBefore(new DateTime(2019, 09, 29));
            Log.Debug($"MonthsFrom result 3: {res}");
            Assert.AreEqual(0, res);
            res = new DateTime(2019, 10, 29).MonthsBefore(new DateTime(2019, 09, 29));
            Log.Debug($"MonthsFrom result 4: {res}");
            Assert.AreEqual(-1, res);
            res = new DateTime(2014, 1, 1).MonthsBefore(new DateTime(2014, 2, 1));
            Log.Debug($"MonthsFrom result 5: {res}");
            Assert.AreEqual(1, res);
            res = new DateTime(2014, 1, 1).MonthsBefore(new DateTime(2014, 1, 31));
            Log.Debug($"MonthsFrom result 6: {res}");
            Assert.AreEqual(0, res);
            res = new DateTime(2014, 1, 1).MonthsBefore(new DateTime(2014, 2, 2));
            Log.Debug($"MonthsFrom result 7: {res}");
            Assert.AreEqual(1, res);

            // 31 Jan to 28 Feb
            Assert.AreEqual(1, new DateTime(2014, 1, 31).MonthsBefore(new DateTime(2014, 2, 28)));
            // Leap year 29 Feb to 29 Mar
            Assert.AreEqual(1, new DateTime(2012, 2, 29).MonthsBefore(new DateTime(2012, 3, 29)));
            // Whole year minus a day
            Assert.AreEqual(11, new DateTime(2012, 1, 1).MonthsBefore(new DateTime(2012, 12, 31)));
            // Whole year
            Assert.AreEqual(12, new DateTime(2012, 1, 1).MonthsBefore(new DateTime(2013, 1, 1)));
            // 29 Feb (leap) to 28 Feb (non-leap)
            Assert.AreEqual(12, new DateTime(2012, 2, 29).MonthsBefore(new DateTime(2013, 2, 28)));
            // 100 years
            Assert.AreEqual(1200, new DateTime(2000, 1, 1).MonthsBefore(new DateTime(2100, 1, 1)));
            // Same date
            Assert.AreEqual(0, new DateTime(2014, 8, 5).MonthsBefore(new DateTime(2014, 8, 5)));
            // Past date
            Assert.AreEqual(-6, new DateTime(2012, 1, 1).MonthsBefore(new DateTime(2011, 6, 10)));

        }
    }
}
