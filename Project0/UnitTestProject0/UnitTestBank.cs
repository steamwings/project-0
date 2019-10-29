using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Project0;
using Serilog;
using System.Threading.Tasks;

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
        }

        [TestMethod]
        public void TestAddDelete()
        {
            UnitTesting.SetupTesting();
            CreateCustomer();
            Task<bool> removeCustomer = Task.Run(() => Bank.RemoveCustomer());
            UnitTesting.AddInput("yes");
            Assert.IsTrue(removeCustomer.GetAwaiter().GetResult());
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
        }

       // [TestMethod]
        public void TestAddDepositDelete()
        {
            UnitTesting.SetupTesting();
            CreateCustomer();
            Task deposit = Task.Run(() => Bank.MainMenu());
            UnitTesting.AddInput("view");
            UnitTesting.AddInput("deposit");
            string info = UnitTesting.GetOutputForInput("45.55");
            Log.Debug($"info:{info}");
            UnitTesting.AutoConsumeOutput = true;
            UnitTesting.AddInput("return");
            UnitTesting.AddInput("close");
            UnitTesting.AddInput("yes");
            UnitTesting.AddInput("yes");
            Assert.IsTrue(info.Contains("45.55"));
        }

        public void TestTransfer()
        {
            UnitTesting.SetupTesting();

        }

    }
}
