using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Project0;
using System;
using System.IO;

namespace UnitTestProject0
{

    class HelperMethods
    {
        private static ILogger<HelperMethods> logger = (new LoggerFactory()).CreateLogger<HelperMethods>();
        public static bool TestGetDollarAmount(string input, double exp)
        {
            logger.LogInformation($"TestGetDollarAmount\ninput:{input}|exp:{exp}");
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
            Assert.IsTrue(HelperMethods.TestGetDollarAmount("45", 45));
        }

        [TestMethod]
        public void TestGetDollarAmountFraction()
        {
            Assert.IsTrue(HelperMethods.TestGetDollarAmount("45.55", 45.55));
        }
    }

    [TestClass]
    public class NegativeUnitTestsMethods
    {
        [TestMethod]
        public void TestGetDollarAmount()
        {
            Assert.IsTrue(HelperMethods.TestGetDollarAmount("45.555", 45.56));
        }
    }
}
