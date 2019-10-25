using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project0
{
    public class Bank
    {
        private static readonly int minPassLen = 5;
        private static int nextAccountId = 1;
        private static Customer currentCustomer;
        private static List<Customer> customers = new List<Customer>();

        public static decimal InterestRate { get; private set; }

        public static void Main()
        {
            Log.Logger = new LoggerConfiguration() // Initialize Serilog
#if DEBUG
            .MinimumLevel.Debug()
            .WriteTo.Console()
#else
            .MinimumLevel.Information()
#endif
            .WriteTo.File("C:\\ProgramData\\Temp\\Bank\\logs\\bank.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            Log.Information("Bank console program started.");
            try
            {
                while (true) // Outermost loop
                {
                    while (!Login()) ;

                    while (true) // User logged in loop
                    {
                        bool exitLoggedIn = false;
                        Console.Clear();
                        Console.WriteLine(currentCustomer.DisplayAllAccounts());
                        Console.WriteLine(Properties.Resources.MainMenuOptions);
                        var r = ConsoleUtil.GetResponse("view", "new", "transfer", "close", "exit");
                        switch (r)
                        {
                            case "view":
                                View();
                                break;
                            case "new":
                                AddAccount();
                                break;
                            case "transfer":
                                Transfer();
                                break;
                            case "close":
                                if (RemoveCustomer()) exitLoggedIn = true;
                                break;
                            case "exit":
                                exitLoggedIn = true;
                                break;
                            default:
                                break;
                        }
                        if (exitLoggedIn) break;
                    }
                    currentCustomer = null;
                    ConsoleUtil.DisplayWait(Properties.Resources.Goodbye);
                }
                
            }
            catch (Exception e)
            {
                ConsoleUtil.Display(Properties.Resources.ServiceUnavailable);
                Log.Error("Unexpected error.", e);
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static bool Login()
        {
            ConsoleUtil.Display(Properties.Resources.Welcome + "\n\n");
            Console.WriteLine(Properties.Resources.ExistingAccountOrRegister);
            var r = ConsoleUtil.GetResponse("login", "register");
            if (r == "login")
            {
                return LoginCustomer();
            }
            else if (r == "register")
            {
                CreateCustomer();
                return true;
            }
            return false;
        }

        public static void View()
        {
            bool exitViewAccount = false;
            IAccount acc = SelectAccount<IAccount>(Properties.Resources.SelectAccountView);
            List<string> options = new List<string>();
            if (acc is IDebt && (((IDebt)acc).AmountOwed() > 0))
            {
                options.Add("payment");
            }
            if (acc is ITransfer)
            {
                options.Add("deposit");
                options.Add("withdraw");
            }
            options.Add("return");
            while (!exitViewAccount) // Viewing account loop
            {
                bool success = true;
                ConsoleUtil.Display(acc.Info());
                switch (ConsoleUtil.GetResponse(options))
                {
                    case "payment":
                        success = ((IDebt)acc).MakePayment(ConsoleUtil.GetDollarAmount());
                        break;
                    case "deposit":
                        ((ITransfer)acc).Deposit(ConsoleUtil.GetDollarAmount());
                        break;
                    case "withdraw":
                        success = ConsoleUtil.PrintWithdrawalStatus(((ITransfer)acc).Withdraw(ConsoleUtil.GetDollarAmount()));
                        break;
                    case "close":
                        exitViewAccount = success = RemoveAccount(acc);
                        break;
                    default:
                        exitViewAccount = true;
                        break;
                }
                ConsoleUtil.PrintOperationStatus(success);
            }
        }

        public static bool LoginCustomer()
        {
            ConsoleUtil.Display(Properties.Resources.EnterUsername);
            var uname = ConsoleUtil.GetString();
            var matches = from c in customers where c.Username == uname select c;

            if (matches.Count() != 1)
            {
                ConsoleUtil.DisplayWait(Properties.Resources.UserNotFound);
                return false;
            }
            else
            {
                Customer c = matches.Single();
                ConsoleUtil.Display(Properties.Resources.EnterPassword);
                int tries = 3;
                while(!c.Login(ConsoleUtil.GetPass(minPassLen)))
                {
                    if (--tries == 0) return false;
                    Console.WriteLine(Properties.Resources.PasswordIncorrect);
                }
                currentCustomer = c;
                return true;
            }
        }

        public static void CreateCustomer()
        {
            ConsoleUtil.Display(Properties.Resources.CreateUsername);
            var uname = ConsoleUtil.GetString();
            while(customers.Where(c=> c.Username == uname).Any())
            {
                ConsoleUtil.Display(Properties.Resources.UsernameUnavailable);
                uname = ConsoleUtil.GetString();
            }
            ConsoleUtil.Display(Properties.Resources.CreatePassword);
            currentCustomer = new Customer(uname, ConsoleUtil.GetPass(minPassLen));
            customers.Add(currentCustomer);
            Log.Information($"Customer \"{uname}\" created.");
            AddAccount();
        }

        public static bool RemoveCustomer()
        {
            if (currentCustomer.FundsOwed())
            {
                ConsoleUtil.DisplayWait(Properties.Resources.CannotCloseOutFundsOwed);
                return false;
            }

            ConsoleUtil.Display(Properties.Resources.AreYouSureDeleteCustomer);
            if (ConsoleUtil.GetConfirm())
            {
                currentCustomer.RemoveAllAccounts();
                Log.Information($"Customer \"{currentCustomer.Username}\" deleted.");
                customers.Remove(currentCustomer);
                return true;
            }
            else return false;
        }

        public static void AddAccount()
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
                    Log.Error(Properties.Resources.ErrorInvalidProgramFlow);
                    throw new BankException();
            }
            Console.WriteLine(Properties.Resources.GiveAccountName);
            a.Name = ConsoleUtil.GetString();
            currentCustomer.AddAccount(a);
            return;
        }

        public static bool RemoveAccount(IAccount acc)
        {
            if (currentCustomer.AccountCount() == 1)
            {
                ConsoleUtil.Display(Properties.Resources.DeletingFinalAccount);
                return RemoveCustomer();
            }
            if (acc.Balance < 0)
            {
                ConsoleUtil.DisplayWait(Properties.Resources.CannotCloseFundsOwed);
                return false;
            }
            ConsoleUtil.Display(Properties.Resources.AreYouSureDeleteAccount);
            if (ConsoleUtil.GetConfirm())
            {
                currentCustomer.RemoveAccount(acc);
            }
            else return false;
            return true;
        }

        public static TAccount SelectAccount<TAccount>(string msg) where TAccount : IAccount
        {
            IEnumerable<string> accNames = currentCustomer.GetAccountNames<IAccount>();
            if (accNames.Count() == 1) return (TAccount)currentCustomer.GetAccount(accNames.First());
            Console.WriteLine(msg);
            return (TAccount)currentCustomer.GetAccount(ConsoleUtil.GetResponse(accNames));
        }

        public static void Transfer()
        {
            if (currentCustomer.AccountCount() < 2)
            {
                Console.WriteLine(Properties.Resources.TransferRequiresTwoAccounts);
                return;
            }
            // IEnumerable<string> anames = from a in accs select a.Name;
            ITransfer from = SelectAccount<ITransfer>(Properties.Resources.SelectAccountTransferFrom);
            currentCustomer.RemoveAccount(from);
            IAccount to = SelectAccount<IAccount>(Properties.Resources.SelectAccountTransferTo);
            currentCustomer.AddAccount(from);
            Console.WriteLine(Properties.Resources.TransferAmount);
            decimal amt = ConsoleUtil.GetDollarAmount();

            if (!(to is IDebt) && !(to is ITransfer))
            {
                Log.Warning("Unknown account type: " + to.GetType().ToString());
                Console.WriteLine(Properties.Resources.IncompatibleAccount);
                return;
            }
            else if (to is IDebt && ((IDebt)to).AmountOwed() < amt) {
                if(!ConsoleUtil.GetConfirm(Properties.Resources.TransferNotMoreThanOwedConfirm))
                {
                    Console.WriteLine(Properties.Resources.OperationCancelled);
                    return;
                }
                amt = ((IDebt)to).AmountOwed();
            }

            if (ConsoleUtil.PrintWithdrawalStatus(from.Withdraw(amt)))
            {
                if(to is ITransfer)
                {
                    ((ITransfer)to).Deposit(amt);
                } else if(to is IDebt)
                {
                    ((IDebt)to).MakePayment(amt);
                }
                Console.WriteLine($"{amt} {Properties.Resources.WasTransferredSuccessfully}");
                Console.WriteLine(Properties.Resources.OperationComplete);
                return;
            }

            Console.WriteLine(Properties.Resources.TransferWithdrawalFailed);
        }
    }
}
