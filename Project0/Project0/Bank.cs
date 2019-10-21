using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project0
{
    public class Bank
    {
        private static int nextAccountId = 1;
        private static Customer currentCustomer;
        private static List<Customer> customers = new List<Customer>();
        private static ILogger<Bank> logger = (new LoggerFactory()).CreateLogger<Bank>();
        public static double InterestRate { get; private set; }

        public static void Main()
        {
            logger.LogInformation("Bank console program started.");
            try
            {
                while (true) // Main loop, no user logged-in
                {
                    Display(Properties.Resources.Welcome + "\n\n");
                    if (Login())
                    {
                        while (true) // Main loop, user logged in
                        {
                            bool exit = false;
                            Display(Properties.Resources.MainMenuOptions);
                            var r = ConsoleUtil.GetResponse("view", "new", "transfer", "exit");
                            switch (r)
                            {
                                case "view":
                                    currentCustomer.DisplayAllAccounts();
                                    IAccount acc = SelectAccount();
                                    List<string> options = new List<string>();
                                    options.Add("exit");
                                    if (acc is IDebt)
                                    {
                                        options.Add("payment");
                                    }
                                    if (acc is CheckingAccount)
                                    {
                                        options.Add("deposit");
                                        options.Add("withdrawal");
                                    }
                                    switch (ConsoleUtil.GetResponse(options))
                                    {
                                        case "payment":
                                            break;
                                        case "deposit":
                                            break;
                                        case "withdrawal":
                                            break;
                                    }
                                    break;
                                case "new":
                                    AddAccount();
                                    break;
                                case "transfer":
                                    Transfer();
                                    break;
                                default:
                                    exit = true;
                                    break;
                            }
                            if(exit) break;
                        }
                        currentCustomer = null;
                        DisplayWait(Properties.Resources.Goodbye);
                    }
                }
            }
            catch (Exception e)
            {
                Display(Properties.Resources.ServiceUnavailable);
                logger.LogError("Unexpected error.", e);
            }
        }

        public static void Display(string s)
        {
            Display(s, 0);
        }
        public static void DisplayWait(string s)
        {
            Display(s, 2000);
        }

        public static void Display(string s, int waitMs)
        {
            Console.Clear();
            Console.WriteLine(s);
            System.Threading.Thread.Sleep(waitMs);
        }

        public static bool Login()
        {
            Display(Properties.Resources.ExistingAccountOrRegister);
            var r = ConsoleUtil.GetResponse("login", "register","exit");
            if(r == "login")
            {
                return LoginCustomer();
            } else if(r == "register")
            {
                return CreateCustomer();
            }
            return false;
        }

        public static bool LoginCustomer()
        {
            Display(Properties.Resources.EnterUsername);
            var uname = ConsoleUtil.GetString();
            var matches = from c in customers where c.Username == uname select c;

            if (matches.Count() != 1)
            {
                DisplayWait(Properties.Resources.UserNotFound);
                return false;
            }
            else
            {
                currentCustomer = matches.Single();
                return true;
            }
        }

        public static bool CreateCustomer()
        {
            Display(Properties.Resources.CreateUsername);
            var uname = ConsoleUtil.GetString();
            while(customers.Where(c=> c.Username == uname).Any())
            {
                Display(Properties.Resources.UsernameUnavailable);
                uname = ConsoleUtil.GetString();
            }
            currentCustomer = new Customer(uname);
            customers.Add(currentCustomer);
            logger.LogInformation($"Customer {uname} created.");
            return AddAccount();
        }

        public static bool RemoveCustomer()
        {
            if (currentCustomer.FundsOwed())
            {
                DisplayWait(Properties.Resources.CannotCloseOutFundsOwed);
                return false;
            }

            Display(Properties.Resources.AreYouSureDeleteCustomer);
            if (ConsoleUtil.GetConfirm())
            {
                currentCustomer.RemoveAllAccounts();
                return true;
            }
            else return false;
        }

        public static bool AddAccount()
        {
            Console.WriteLine(Properties.Resources.WhatAccountType);
            IAccount a;
            switch (ConsoleUtil.GetResponse("checking", "business", "loan", "cd"))
            {
                case "checking":
                    a = new CheckingAccount(nextAccountId++);
                    break;
                case "business":
                    a = new BusinessAccount(nextAccountId++);
                    break;
                case "loan":
                    Console.WriteLine(Properties.Resources.WhatSizeLoan);
                    a = new Loan(nextAccountId++, ConsoleUtil.GetDollarAmount());
                    break;
                case "cd":
                    Console.WriteLine(Properties.Resources.WhatSizeCD);
                    a = new TermDeposit(nextAccountId++, ConsoleUtil.GetDollarAmount());
                    break;
                default:
                    logger.LogError(Properties.Resources.ErrorInvalidProgramFlow);
                    return false;
            }
            Display(Properties.Resources.GiveAccountName);
            a.Name = ConsoleUtil.GetString();
            currentCustomer.AddAccount(a);
            return true;
        }

        public static bool RemoveAccount(IAccount acc)
        {
            if(acc.Balance < 0)
            {
                DisplayWait(Properties.Resources.CannotCloseFundsOwed);
                return false;
            }
            Display(Properties.Resources.AreYouSureDeleteAccount);
            if (ConsoleUtil.GetConfirm())
            {
                currentCustomer.RemoveAccount(acc);
            }
            else return false;
            return true;
        }

        public static IAccount SelectAccount()
        {
            Display(Properties.Resources.SelectAccount);
            return currentCustomer.GetAccount(ConsoleUtil.GetResponse(currentCustomer.GetAccountNames()));
        }

        public static void Transfer()
        {
            
            //Display();
        }
    }
}
