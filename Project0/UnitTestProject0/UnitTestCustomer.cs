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
        }
    }
}
