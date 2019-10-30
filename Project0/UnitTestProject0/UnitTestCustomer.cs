using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project0;

namespace UnitTestProject0
{
    [TestClass]
    public class UnitTestCustomer
    {
        [TestMethod]
        public void TestPasswordHash()
        {
            UnitTesting.SetupTesting();
            var pass = "password";
            Customer c = new Customer("user", pass);
            Assert.IsTrue(c.Login(pass));
            UnitTesting.EndTest();
        }

        [TestMethod]
        public void TestGetAccounts()
        {
            UnitTesting.SetupTesting();
            Customer c = new Customer("user", "pass");
            c.AddAccount(new CheckingAccount(1));
            c.AddAccount(new TermDeposit(2, 4000));
            c.AddAccount(new BusinessAccount(3));
            Assert.IsTrue(c.GetAccounts<IChecking>().Count == 2);
            UnitTesting.EndTest();
        }
    }
}
