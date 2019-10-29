using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Exceptions;
using System.Runtime.CompilerServices;
using Project0;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Threading;

namespace UnitTestProject0
{

    public static class UnitTesting
    {
        private static bool setup = false;
        private static readonly Queue<string> Inputs = new Queue<string>();
        private static readonly Queue<string> Outputs = new Queue<string>();
        private static int inputIndex = 0;
        // When set to true, this property will consume and print program output
        public static bool AutoConsumeOutput { get; set; } = true;
        private static Task printTask;

        public static bool TestRunning { get; set; } = false;

        public static void AddInput(string input) { Inputs.Enqueue(input); }
        public static bool HasOutput() { return Outputs.Count > 0; }
        public static string GetNextOutput() {
            while (!HasOutput()) ;
            return Outputs.Dequeue();
        }

        public static string GetOutputForInput(string input)
        {
            while (Inputs.Count > 0) ; // Program has inputs to consume
            if(AutoConsumeOutput) while (HasOutput()) ; // Outputs need to be auto-consumed
            AutoConsumeOutput = false;
            Inputs.Enqueue(input);
            string s = GetNextOutput();
            AutoConsumeOutput = true;
            return s;
        }
        
        /// <summary>
        /// Resets variables for each test. For the first test, it will create the Serilog logger and remap ConsoleUtil I/O.
        /// </summary>
        /// <param name="autoConsume"> This is true by default. Pass false if you would like 
        /// to call GetNextOutput for each output. </param>
        /// <param name="name"> Automatically supplied name of caller. </param>
        public static void SetupTesting(bool autoConsume = true, [CallerMemberName] string name = "")
        {
            TestRunning = false;
            printTask?.GetAwaiter().GetResult();
            if (!setup)
            {
                Log.Logger = new LoggerConfiguration() // Initialize Serilog
                //.MinimumLevel.Debug()
                .MinimumLevel.Verbose()
                .Enrich.WithExceptionDetails()
                .WriteTo.Debug()
                .WriteTo.File("C:\\git\\Project0\\logs\\bank-test.log", rollingInterval: RollingInterval.Infinite)
                .CreateLogger();
                Log.Information("Log created.");
                ConsoleUtil.Clear = () => { };
                ConsoleUtil.ReadLine = () => {
                    string s = null;
                    if (Inputs.Count > 0)
                    {
                        s = Inputs.Dequeue();
                        Log.Verbose("Inputted: "+s);
                    }
                    return s;
                };
                ConsoleUtil.WriteLine = msg => { Log.Verbose("Program outputted: "+msg); Outputs.Enqueue(msg); };
                ConsoleUtil.Write = ConsoleUtil.WriteLine;
                ConsoleUtil.ReadKey = hide => { return ReadInputChar(); };
                ConsoleUtil.KeyAvailable = () => { return Inputs.Count > 0; };
                setup = true;
            }
            Bank.Reset();
            inputIndex = 0;
            Inputs.Clear();
            Outputs.Clear();
            TestRunning = true;
            AutoConsumeOutput = autoConsume;
            printTask = PrintOutput();
            Log.Information($"Start {name}");
        }

        private static Task PrintOutput()
        {
            return Task.Run(() => {
                while (TestRunning)
                {
                    if (AutoConsumeOutput && HasOutput()) Outputs.Dequeue();
                }
            });
        }

        private static ConsoleKeyInfo ReadInputChar()
        {
            while (Inputs.Count == 0) ;
            string s = Inputs.Peek();
            char c = s[inputIndex++];
            if (inputIndex == s.Length)
            {
                inputIndex = 0;
                Inputs.Dequeue();
            }
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
