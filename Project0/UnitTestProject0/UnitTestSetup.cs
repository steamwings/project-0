using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
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
            //.WriteTo.Console()
            .WriteTo.File("C:\\git\\Project0\\logs\\bank-test.log", rollingInterval: RollingInterval.Infinite)
            .CreateLogger();
            Log.Information("Log created.");
            //Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            Debug.WriteLine("Log created?");
            setup = true;
        }
    }
}
