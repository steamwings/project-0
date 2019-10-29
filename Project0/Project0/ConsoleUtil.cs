using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Project0
{
    /// <summary>
    /// Static utility functions to help with output and user input.
    /// </summary>
    public static class ConsoleUtil
    {
        public delegate string DelReadLine();
        public delegate ConsoleKeyInfo DelReadKey(bool hide);
        public delegate void DelWrite(string msg);
        public delegate void DelVoid();
        public delegate bool DelBool();
        // Basic function pointers to control input/output flow
        public static DelVoid Clear = () => { Console.Clear(); };
        public static DelReadLine ReadLine = () => { return Console.ReadLine(); };
        public static DelReadKey ReadKey = hide => { return Console.ReadKey(hide); };
        public static DelWrite WriteLine = msg => {Console.WriteLine(msg); };
        public static DelWrite Write = msg => { Console.Write(msg); };
        public static DelBool KeyAvailable = () => { return Console.KeyAvailable; };
        
        public static void Display(string s)
        {
            Clear();
            WriteLine(s);
        }
        public static void DisplayWait(string s)
        {
            Display(s);
            GetAnyKey();
        }

        public static void GetAnyKey()
        {
            WriteLine(Properties.Resources.PressAnyKey);
            while (!KeyAvailable()) ;
            ReadKey(true);
        }

        public static decimal GetDollarAmount()
        {
            var rg = new Regex(@"^\d{0,14}(.\d\d)?$");
            Write(Properties.Resources.EnterDollarAmount + " $");
            var resp = ReadLine();
            while (!rg.IsMatch(resp))
            {
                WriteLine(Properties.Resources.ValidAmount);
                Write("$");
                resp = ReadLine();
            }
            return Decimal.Parse(resp);
        }

        public static void PrintOperationStatus(bool success)
        {
            if (success) WriteLine(Properties.Resources.OperationComplete);
            else WriteLine(Properties.Resources.OperationFailed);
        }

        public static bool PrintTransferResult(TransferResult res)
        {
            switch (res)
            {
                case TransferResult.SuccessBorrowing:
                    WriteLine(Properties.Resources.WithdrawalSuccessBorrow);
                    return true;
                case TransferResult.SuccessNoBorrow:
                    WriteLine(Properties.Resources.WithdrawalSuccessNoBorrow);
                    return true;
                case TransferResult.ImmatureFunds:
                    WriteLine(Properties.Resources.WithdrawalImmatureFunds);
                    break;
                case TransferResult.InsufficientFunds:
                    WriteLine(Properties.Resources.WithdrawalInsufficientFunds);
                    break;
                default:
                    break;
            }
            return false;
        }

        public enum GetResponseOption{
            DisplayResponses,
            DoNotDisplayResponses
        }

        public static bool GetConfirm()
        {
            return GetResponse("yes", "no") == "yes";
        }

        public static bool GetConfirm(string msg)
        {
            WriteLine(msg);
            return GetConfirm();
        }

        public static string GetResponse(params string[] vals){
            return GetResponse(new List<string>(vals));
        }

        public static string GetResponse(List<string> responses)
        {
            if(responses.Count() == 0) Log.Warning("No user options in GetResponse!");
            string s;
            do
            {
                Write(Properties.Resources.PleaseEnter);
                var en = responses.GetEnumerator();
                en.MoveNext();
                Write($"\"{en.Current}\"");
                while (en.MoveNext())
                {
                    Write($" or \"{en.Current}\"");
                }
                WriteLine("");
                s = ReadLine();
            }
            while (!responses.Contains(s));
            return s;
        }

        public static string GetPass(int minPassLen)
        {
            StringBuilder s = new StringBuilder();
            while (true)
            {
                var c = ReadKey(true);
                if (c.Key == ConsoleKey.Enter)
                {
                    if (s.Length < minPassLen)
                    {
                        s.Clear();
                        Clear();
                        WriteLine(Properties.Resources.PasswordLength.Replace("{}", minPassLen.ToString()));
                        WriteLine(Properties.Resources.CreatePassword);
                    }
                    else break;
                }
                else if (c.Key == ConsoleKey.Backspace)
                {
                    if (s.Length > 0)
                    {
                        Write("\b\x1B[1P");
                        s.Remove(s.Length - 1, 1);
                    }
                }
                else
                {
                    Write("*");
                    s.Append(c.KeyChar);
                }
            }
            return s.ToString() ;
        }

        public static string GetString()
        {
            string s;
            do
            {
                s = ReadLine();
            } while (string.IsNullOrEmpty(s));
            return s;
        }

    }
}
