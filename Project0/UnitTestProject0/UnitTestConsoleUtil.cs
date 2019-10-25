using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project0;
using Serilog;
using System;
using System.IO;

namespace UnitTestProject0
{
    class HelperMethods
    {
        public static bool TestGetDollarAmount(string input, double exp)
        {
            Log.Information($"TestGetDollarAmount\ninput:{input}|exp:{exp}");
            Console.SetOut(new StringWriter());
            Console.SetIn(new StringReader(input));
            return exp == ConsoleUtil.GetDollarAmount();
        }
    }

    [TestClass]
    public class PositiveUnitTestsMethods
    {
        [TestMethod]
        public void TestGetDollarAmountBasic()
        {
            UnitTestSetup.SetupTesting();
            Assert.IsTrue(HelperMethods.TestGetDollarAmount("45", 45));
        }

        [TestMethod]
        public void TestGetDollarAmountFraction()
        {
            UnitTestSetup.SetupTesting();
            Assert.IsTrue(HelperMethods.TestGetDollarAmount("45.55", 45.55));
        }
        [TestMethod]
        public void TestGetPass()
        {
            UnitTestSetup.SetupTesting();
            Log.Information($"TestGetPass\n");
            Console.SetOut(new StringWriter());
            Console.SetIn(new StringReader(""));
        }

    }

    [TestClass]
    public class NegativeUnitTestsMethods
    {
        [TestMethod]
        public void TestGetDollarAmount()
        {
            UnitTestSetup.SetupTesting();
            Assert.IsTrue(HelperMethods.TestGetDollarAmount("45.555", 45.56));
        }
    }
}
