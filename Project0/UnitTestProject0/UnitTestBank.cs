using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Project0;
using Serilog;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace UnitTestProject0
{
    [TestClass]
    public class UnitTestBank
    {
        private void CreateCustomer()
        {
            Task newCustomer = Task.Run(() => Bank.CreateCustomer());
            UnitTesting.AddInput("user");
            UnitTesting.AddInput("password\n");
            UnitTesting.AddInput("checking");
            UnitTesting.AddInput("My Checking");
            newCustomer.GetAwaiter().GetResult();
            Log.Debug("CreateCustomer completed.");
        }

        [TestMethod]
        public void TestListAccountTypes()
        {
            UnitTesting.SetupTesting();
            List<string> l = Bank.ListAccountTypes<IAccount>();
            Assert.IsTrue(l.Contains("checking") && l.Contains("business") && l.Contains("loan") && l.Contains("cd"));
            Log.Debug("Part 1 complete");
            l = Bank.ListAccountTypes<IChecking>();
            Assert.IsTrue(l.Contains("checking") && l.Contains("business") && l.Count == 2);
            UnitTesting.EndTest();
        }

        [TestMethod]
        public void TestAddDelete()
        {
            UnitTesting.SetupTesting();
            CreateCustomer();
            Task<bool> removeCustomer = Task.Run(() => Bank.RemoveCustomer());
            UnitTesting.AddInput("yes");
            Assert.IsTrue(removeCustomer.GetAwaiter().GetResult());
            UnitTesting.EndTest();
        }

        public void TestLogin()
        {
            UnitTesting.SetupTesting();
            CreateCustomer();
            Task logoutCustomer = Task.Run(() => Bank.MainMenu());
            UnitTesting.AddInput("logout");
            logoutCustomer.GetAwaiter().GetResult();
            Task<bool> loginCustomer = Task.Run(() => Bank.Login());
            UnitTesting.AddInput("login");
            UnitTesting.AddInput("user");
            UnitTesting.AddInput("password");
            Assert.IsTrue(loginCustomer.GetAwaiter().GetResult());
            UnitTesting.EndTest();
        }

        [TestMethod]
        public void TestAddDepositDelete()
        {
            UnitTesting.SetupTesting();
            Regex re = new Regex(@"\$45\.55");
            UnitTesting.AddWatch(re);
            CreateCustomer();
            Task deposit = Task.Run(() => Bank.MainMenu());
            UnitTesting.AddInput("view");
            UnitTesting.AddInput("deposit");
            UnitTesting.AddInput("45.55");
            UnitTesting.AddInput("return");
            UnitTesting.AddInput("close");
            UnitTesting.AddInput("yes");
            UnitTesting.AddInput("yes");
            UnitTesting.AddInput("\n");
            deposit.GetAwaiter().GetResult();
            int res = UnitTesting.WatchCount(re);
            UnitTesting.EndTest();
            Log.Information($"Recorded {res} occurrences.");
            Assert.IsTrue(res > 0);
        }

    }
}
