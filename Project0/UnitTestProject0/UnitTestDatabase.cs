using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project0;
using Serilog;


namespace UnitTestProject0
{
    [TestClass]
    public class UnitTestDatabase
    {
        //[TestMethod]
        public void TestCreateCustomer()
        {
            UnitTesting.SetupTesting();
            Assert.IsTrue(true);
        }

    }
}
