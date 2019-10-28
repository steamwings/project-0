using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project0;
using Serilog;
using System.Threading.Tasks;

namespace UnitTestProject0
{
 
    [TestClass]
    public class UnitTestConsoleUtil
    {
        public static bool TestGetDollarAmount(string input, decimal exp)
        {
            Log.Information($"TestGetDollarAmount\ninput:{input}|exp:{exp}");
            UnitTesting.AddInput(input);
            return exp == ConsoleUtil.GetDollarAmount();
        }

        [TestMethod]
        public void TestGetDollarAmountBasic()
        {
            UnitTesting.SetupTesting();
            Assert.IsTrue(TestGetDollarAmount("45", 45));
        }

        [TestMethod]
        public void TestGetDollarAmountFraction()
        {
            UnitTesting.SetupTesting();
            Assert.IsTrue(TestGetDollarAmount("45.55", 45.55M));
        }

        [TestMethod]
        public void TestNegativeGetDollarAmountFraction()
        {
            UnitTesting.SetupTesting();
            Task<decimal> getAmt = Task.Run(() => ConsoleUtil.GetDollarAmount());
            UnitTesting.AddInput("45.555");
            UnitTesting.AddInput("45.67");
            decimal res = getAmt.GetAwaiter().GetResult();
            Assert.AreEqual(45.67M, res);
        }

        [TestMethod]
        public void TestGetPass()
        {
            UnitTesting.SetupTesting();
            int minPassLen = 5;
            string password = "password";
            Task<string> getPass = Task.Run(() => ConsoleUtil.GetPass(minPassLen));
            UnitTesting.AddInput("p\n");
            UnitTesting.AddInput(password + "\n");
            string res = getPass.GetAwaiter().GetResult();
            Assert.IsTrue(password == res);
            UnitTesting.TestRunning = false;
            Log.Information($"Done TestGetPass\n");
        }

    }
}
