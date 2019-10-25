﻿using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using static Project0.ITransfer;

namespace Project0
{
    /// <summary>
    /// Static utility functions to help with output and user input.
    /// </summary>
    public static class ConsoleUtil
    {
        public static void Display(string s)
        {
            Console.Clear();
            Console.WriteLine(s);
        }
        public static void DisplayWait(string s)
        {
            Display(s);
            GetAnyKey();
        }

        public static int GetDollarAmount()
        {
            //var rg = new Regex(@"^\d{0,14}(.00)?$");
            Console.Write(Properties.Resources.EnterDollarAmount + " ");
            var resp = Console.ReadLine();
            int amt;
            while (!int.TryParse(resp, out amt)/* || !rg.IsMatch(resp)*/)
            {
                Console.WriteLine(Properties.Resources.ValidAmount);
                resp = Console.ReadLine();
            }
            return amt;
        }

        public static void PrintOperationStatus(bool success)
        {
            if (success) Console.WriteLine(Properties.Resources.OperationComplete);
            else Console.WriteLine(Properties.Resources.OperationFailed);
        }

        public static bool PrintWithdrawalStatus(WithdrawalResult res)
        {
            switch (res)
            {
                case WithdrawalResult.SuccessBorrowing:
                    Console.WriteLine(Properties.Resources.WithdrawalSuccessBorrow);
                    return true;
                case WithdrawalResult.SuccessNoBorrow:
                    Console.WriteLine(Properties.Resources.WithdrawalSuccessNoBorrow);
                    return true;
                case WithdrawalResult.ImmatureFunds:
                    Console.WriteLine(Properties.Resources.WithdrawalImmatureFunds);
                    break;
                case WithdrawalResult.InsufficientFunds:
                    Console.WriteLine(Properties.Resources.WithdrawalInsufficientFunds);
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

        public static void GetAnyKey()
        {
            Console.WriteLine(Properties.Resources.PressAnyKey);
            while (!Console.KeyAvailable);
            Console.Read();
        }

        public static bool GetConfirm()
        {
            return GetResponse("yes", "no") == "yes";
        }

        public static bool GetConfirm(string msg)
        {
            Console.WriteLine(msg);
            return GetConfirm();
        }

        public static string GetResponse(IEnumerable<string> r)
        {
            return GetResponse(r.ToList());
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
                Console.Write(Properties.Resources.PleaseEnter);
                var en = responses.GetEnumerator();
                en.MoveNext();
                Console.Write($"\"{en.Current}\"");
                while (en.MoveNext())
                {
                    Console.Write($" or \"{en.Current}\"");
                }
                Console.WriteLine();
                s = Console.ReadLine();
            }
            while (!responses.Contains(s));
            return s;
        }

        public static string GetPass(int minPassLen)
        {
            StringBuilder s = new StringBuilder();
            while (true)
            {
                var c = Console.ReadKey();
                if (c.Key == ConsoleKey.Enter)
                    break;
                else if (c.Key == ConsoleKey.Backspace)
                    s.Remove(s.Length - 1, 1);
                else s.Append(c);
            }
            return s.ToString() ;
        }

        public static string GetString()
        {
            string s;
            do
            {
                s = Console.ReadLine();
            } while (string.IsNullOrEmpty(s));
            return s;
        }

    }
}
