using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Exceptions;
using System.Diagnostics;

namespace UnitTestProject0
{
    [TestClass]
    public class UnitTestSetup
    {
        private static bool setup = false;
        public static void SetupTesting()
        {
            if (setup) return;
            Log.Logger = new LoggerConfiguration() // Initialize Serilog
            .MinimumLevel.Debug()
            //.MinimumLevel.Verbose()
            .Enrich.WithExceptionDetails()
            .WriteTo.File("C:\\git\\Project0\\logs\\bank-test.log", rollingInterval: RollingInterval.Infinite)
            .CreateLogger();
            Log.Information("Log created.");
            Debug.WriteLine("Debug: Log created.");
            setup = true;
        }
    }
}
