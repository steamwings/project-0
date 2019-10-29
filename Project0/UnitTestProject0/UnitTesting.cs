using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Serilog.Exceptions;
using System.Runtime.CompilerServices;
using Project0;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;
using System.Linq;

namespace UnitTestProject0
{
    public static class UnitTesting
    {
        private static bool setup = false;
        private static readonly Queue<string> Inputs = new Queue<string>();
        private static readonly Queue<string> Outputs = new Queue<string>();
        private static int inputIndex = 0;
        // When set to true, this property will consume program output
        public static bool AutoConsumeOutput { get; set; } = true;
        private static Task printTask;
        private static Mutex watchMutex = new Mutex();
        private static Dictionary<Regex, int> watchList = new Dictionary<Regex, int>();

        public static bool TestDone { get; set; } = false;

        public static void AddInput(string input) { Inputs.Enqueue(input); }
        public static bool HasOutput() { return Outputs.Count > 0; }
        public static string GetNextOutput() {
            while (!HasOutput()) ;
            return Outputs.Dequeue();
        }
        
        public static void AddWatch(Regex re)
        {
            watchMutex.WaitOne();
            watchList.Add(re, 1);
            watchMutex.ReleaseMutex();
        }
        public static int WatchCount(Regex re)
        {
            watchMutex.WaitOne();
            int res = watchList[re];
            watchMutex.ReleaseMutex();
            return res;
        }

        /// <summary>
        /// Resets variables for each test. For the first test, it will create the Serilog logger and remap ConsoleUtil I/O.
        /// </summary>
        /// <param name="autoConsume"> This is true by default. Pass false if you would like 
        /// to call GetNextOutput for each output. </param>
        /// <param name="name"> Automatically supplied name of caller. </param>
        public static void SetupTesting(bool autoConsume = true, [CallerMemberName] string name = "")
        {
            //if(!(printTask is null)) 
            if (!setup)
            {
                Log.Logger = new LoggerConfiguration() // Initialize Serilog
                .MinimumLevel.Debug()
                //.MinimumLevel.Verbose()
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
            TestDone = false;
            AutoConsumeOutput = autoConsume;
            printTask = ConsumeOutput();
            Log.Information($"Start {name}");
        }

        public static void EndTest([CallerMemberName] string name = "")
        {
            Log.Information($"Finished {name}");
            TestDone = true;
            printTask.GetAwaiter().GetResult();
            watchList.Clear();
            inputIndex = 0;
            Bank.Reset();
            Inputs.Clear();
            Outputs.Clear();
        }

        private static Task ConsumeOutput()
        {
            return Task.Run(() => {
                while (!TestDone)
                {
                    if (AutoConsumeOutput && watchMutex.WaitOne(20))
                    {
                        if (Outputs.Count != 0)
                        {
                            string o = Outputs.Dequeue();
                            foreach(var re in watchList.Keys.ToList())
                            {
                                if (re.IsMatch(o))
                                {
                                    Log.Debug("Regex match on " + o);
                                    watchList[re]++;
                                }
                            }
                        }
                       watchMutex.ReleaseMutex();
                    }
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
