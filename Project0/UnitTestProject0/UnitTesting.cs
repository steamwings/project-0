using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Exceptions;
using System.Runtime.CompilerServices;
using Project0;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace UnitTestProject0
{

    public static class UnitTesting
    {
        private static bool setup = false;
        private static readonly Queue<string> Inputs = new Queue<string>();
        private static readonly Queue<string> Outputs = new Queue<string>();
        private static int inputIndex = 0;

        public static bool TestRunning { get; set; } = false;

        public static void AddInput(string input) { Inputs.Enqueue(input); }
        public static bool HasOutput() { return Outputs.Count > 0; }
        public static string NextOutput() { return Outputs.Dequeue();  }

        
        private static Task PrintOutput() { return Task.Run(() => {
            while (UnitTesting.TestRunning)
            {
                if (UnitTesting.HasOutput()) Log.Debug("Program outputted:" + UnitTesting.NextOutput());
            }
        });
        }

        public static void SetupTesting([CallerMemberName] string name = "")
        {
            TestRunning = false;
            if (!setup)
            {
                Log.Logger = new LoggerConfiguration() // Initialize Serilog
                .MinimumLevel.Debug()
                //.MinimumLevel.Verbose()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                //.WriteTo.File("C:\\git\\Project0\\logs\\bank-test.log", rollingInterval: RollingInterval.Infinite)
                .CreateLogger();
                Log.Information("Log created.");
                ConsoleUtil.Clear = () => { };
                ConsoleUtil.ReadLine = () => { return (Inputs.Count > 0) ? Inputs.Dequeue() : ""; };
                ConsoleUtil.WriteLine = msg => { Outputs.Enqueue(msg); };
                ConsoleUtil.ReadKey = hide => { return ReadInputChar(); };
                setup = true;
            }
            inputIndex = 0;
            Inputs.Clear();
            Outputs.Clear();
            TestRunning = true;
            PrintOutput(); // This might need to be optional for tests in the future
            Log.Information($"Start {name}");
        }

        private static ConsoleKeyInfo ReadInputChar()
        {
            while (Inputs.Count == 0) ;
            string s = Inputs.Peek();
            if (inputIndex == s.Length)
            {
                inputIndex = 0;
                Inputs.Dequeue();
                return ReadInputChar();
            }
            char c = s[inputIndex++];
            var key = c switch
            {
                '\n' => ConsoleKey.Enter,
                '\b' => ConsoleKey.Backspace,
                _ => ConsoleKey.A,
            };
            return new ConsoleKeyInfo(c, key, false, false, false);
        }
    }
}
