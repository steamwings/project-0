using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Project0
{
    class Bank
    {
        private static int nextAccountId = 1;
        private static Customer current;
        private static List<Customer> customers = new List<Customer>();
        private static List<IAccount> accounts = new List<IAccount>();
        private static ILogger logger;

        static void Main(string[] args)
        {
            ILoggerFactory loggerFactory = new LoggerFactory();
            ILogger logger = loggerFactory.CreateLogger<Bank>();

            try
            {
                while (true)
                {
                    Console.WriteLine(Properties.Resources.Welcome + "\n\n");
                    if (Login())
                    {
                        Console.WriteLine(Properties.Resources.MainMenuOptions);
                        var r = GetResponse("view", "create", "exit");
                        if (r == "view")
                        {
                            Console.WriteLine("Viewing.");
                        }
                        else if (r == "create")
                        {
                            Console.WriteLine("Creating.");
                        }
                    }
                    current = null;
                    Console.WriteLine(Properties.Resources.Goodbye);
                }
            }catch(Exception e)
            {
                Console.WriteLine(Properties.Resources.ServiceUnavailable);
                logger.LogError("Unexpected error.", e);
            }
        }
        static bool Login()
        {
            Console.WriteLine(Properties.Resources.ExistingAccountOrRegister);
            var r = GetResponse("login", "register","exit");
            if(r == "login")
            {
                return LoginAccount();
            } else if(r == "register")
            {
                CreateAccount();
                return true;
            }
            return false;
        }

        static bool LoginAccount()
        {
            Console.WriteLine(Properties.Resources.EnterUsername);
            var c = customers.Where(c => c.Username == Console.ReadLine());
            if (c.Count() == 0)
            {
                Console.WriteLine(Properties.Resources.UserNotFound);
                return false;
            }
            else
            {
                current = c.Single();
                return true;
            }
        }

        static void CreateAccount()
        {
            Console.WriteLine(Properties.Resources.CreateUsername);
            var uname = Console.ReadLine();
            while(customers.Where(c=> c.Username == uname).Any())
            {
                Console.WriteLine(Properties.Resources.UsernameUnavailable);
                uname = Console.ReadLine();
            }
            current = new Customer(uname);
            customers.Add(current);
            AddAccount();
        }

        static void AddAccount()
        {
            Console.WriteLine(Properties.Resources.WhatAccountType);
            switch (GetResponse("Checking", "Business", "Loan", "CD"))
            {
                case "Checking":
                    current.AddAccount(new BasicAccount(nextAccountId++));
                    break;
                case "Business":
                    current.AddAccount(new BusinessAccount(nextAccountId++));
                    break;
                case "Loan":
                    Console.WriteLine(Properties.Resources.WhatSizeLoan);
                    current.AddAccount(new Loan(nextAccountId++, GetDollarAmount()));
                    break;
                case "CD":
                    Console.WriteLine(Properties.Resources.WhatSizeCD);
                    current.AddAccount(new TermDeposit(nextAccountId++, GetDollarAmount()));
                    break;
            }
        }

        static void Transfer(IAccount from, IAccount to, int amount)
        {
            if (!from.Withdraw(amount)) Console.WriteLine(Properties.Resources.WithdrawalFailed);
            else
            {
                to.Deposit(amount);
                from.DisplayInfo();
            }

        }
        static int GetDollarAmount()
        {
            var rg = new Regex(@"^\d{0,14}(.00)?$");
            var resp = Console.ReadLine();
            while (!rg.IsMatch(resp))
            {
                Console.WriteLine(Properties.Resources.ValidAmount);
                resp = Console.ReadLine();
            }
            return int.Parse(resp);
        }

        static string GetResponse(params string[] vals)
        {
            List<string> responses = new List<string>(vals);
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
    }
}
