﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project0;
using Serilog;

namespace UnitTestProject0
{
    [TestClass]
    public class UnitTestTransaction
    {
        [TestMethod]
        public void TestFormatTransactions()
        {
            UnitTesting.SetupTesting();
            //Customer c = new Customer("name", "pass");
            CheckingAccount ca = new CheckingAccount(3);
            //c.AddAccount(ca);
            ca.Deposit(45.55M);
            ca.Deposit(67);
            ca.Withdraw(56);
            ca.Deposit(4.50M);
            Log.Information("Format transactions test:\n" +Transaction.FormatTransactions(ca.Transactions));
            Assert.IsTrue(true);
            UnitTesting.EndTest();
        }
    }
}
