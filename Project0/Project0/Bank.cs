﻿using Serilog;
using Serilog.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project0
{
    public static class Bank
    {
        private static readonly int minPassLen = 5;
        private static int nextAccountId = 1;
        private static Customer currentCustomer;
        //P1TODO Use repository pattern with DB
        private static readonly List<Customer> customers = new List<Customer>();

        public static decimal InterestRate { get; private set; } = .01M;
#if DEBUG
        public static int CustomerCount()
        {
            return customers.Count();
        }

        public static void Reset()
        {
            nextAccountId = 1;
            currentCustomer = null;
            customers.Clear();
        }
#endif
        public static void Main()
        {
            Log.Logger = new LoggerConfiguration() // Initialize Serilog
#if DEBUG
            .MinimumLevel.Debug()
            .WriteTo.Console()
#else
            .MinimumLevel.Information() //default
#endif
            .Enrich.WithExceptionDetails()
            .WriteTo.File("C:\\ProgramData\\Temp\\Bank\\logs\\bank.log", rollingInterval: RollingInterval.Day)
            .CreateLogger();

            Log.Information("Bank console program started.");

#if DEBUG
            ConsoleUtil.WriteLine += msg => { Log.Verbose("WriteLine:"+msg); };
#endif
            customers.Add(new Customer("admin", "admin")); //P1TODO Remove

            try
            {
                while (true) // Outermost loop
                {
                    while (!Login()) ;
                    MainMenu();
                }
                
            }
            catch (Exception e)
            {
                ConsoleUtil.Display(Properties.Resources.ServiceUnavailable);
                Log.Error(e, "Unexpected error!");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static void MainMenu()
        {
            if (currentCustomer is null) Log.Warning("Main menu: No current customer.");
            while (true) // User logged in loop
            {
                bool exitLoggedIn = false;
                ConsoleUtil.Clear();
                ConsoleUtil.WriteLine(currentCustomer.DisplayAllAccounts());
                ConsoleUtil.WriteLine(Properties.Resources.MainMenuOptions);
                List<string> mainMenu = new List<string>() { "view", "new", "transfer", "close", "logout" };
                if (currentCustomer.Username == "admin")
                {
                    mainMenu.Add("age accounts");
                    mainMenu.Add("compound");
                }
                var r = ConsoleUtil.GetResponse(mainMenu);
                switch (r)
                {
                    case "view":
                        View();
                        break;
                    case "new":
                        AddAccount<IAccount>();
                        break;
                    case "transfer":
                        Transfer();
                        break;
                    case "close":
                        if (RemoveCustomer()) exitLoggedIn = true;
                        break;
                    case "logout":
                        exitLoggedIn = true;
                        break;
                    case "compound":
                        CompoundInterest();
                        break;
                    case "age accounts":
                        ConsoleUtil.WriteLine("How many months?");
                        AgeAccounts(int.Parse(ConsoleUtil.ReadLine()));
                        break;
                    default:
                        break;
                }
                if (exitLoggedIn) break;
            }
            currentCustomer = null;
            ConsoleUtil.DisplayWait(Properties.Resources.Goodbye);
        }

        public static bool Login()
        {
            ConsoleUtil.Display("\n" + Properties.Resources.Welcome + "\n\n");
            ConsoleUtil.WriteLine(Properties.Resources.ExistingAccountOrRegister);
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
            options.Add("transactions");
            if(acc is IChecking)
            {
                options.Add("deposit");
                options.Add("withdraw");
            }
            if (acc is ITransfer && currentCustomer.AccountCount() > 1
                && (!(acc is ITerm) || ((ITerm)acc).IsMature))
            {
                options.Add("transfer");
            }
            if (acc is IDebt && (((IDebt)acc).AmountOwed() > 0))
            {
                options.Add("payment");
            }
            else
            {
                options.Add("close");
            }
            options.Add("return");
            while (!exitViewAccount) // Viewing account loop
            {
                bool success = true;
                ConsoleUtil.Display(acc.Info());
                switch (ConsoleUtil.GetResponse(options))
                {
                    case "transactions":
                        //foreach (var t in acc.Transactions) ConsoleUtil.WriteLine(t.ToString());
                        ConsoleUtil.WriteLine(Transaction.FormatTransactions(acc.Transactions));
                        ConsoleUtil.GetAnyKey();
                        break;
                    case "payment":
                        success = ((IDebt)acc).MakePayment(ConsoleUtil.GetDollarAmount());
                        break;
                    case "deposit":
                        ((IChecking)acc).Deposit(ConsoleUtil.GetDollarAmount());
                        break;
                    case "withdraw":
                        success = ConsoleUtil.PrintTransferResult(((IChecking)acc).Withdraw(ConsoleUtil.GetDollarAmount()));
                        break;
                    case "transfer":
                        success = Transfer((ITransfer)acc);
                        break;
                    case "close":
                        exitViewAccount = success = CloseAccount(acc);
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
                    ConsoleUtil.WriteLine(Properties.Resources.PasswordIncorrect);
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
            AddAccount<IAccount>();
        }

        public static bool RemoveCustomer()
        {
            if (currentCustomer.HasDebt())
            {
                ConsoleUtil.DisplayWait(Properties.Resources.CannotCloseOutFundsOwed);
                return false;
            }


            if (currentCustomer.HasFunds())
            {
                ConsoleUtil.Display(Properties.Resources.ConfirmCloseWithFunds);
                if (!ConsoleUtil.GetConfirm()) return false;
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

        public static List<string> ListAccountTypes<TAccount>() where TAccount : IAccount
        {
            List<string> l = new List<string>();
            foreach (var t in typeof(TAccount).Assembly.GetTypes().Where(type => typeof(TAccount).IsAssignableFrom(type)))
            {
                if (t.Name == "CheckingAccount") l.Add("checking");
                else if (t.Name == "BusinessAccount") l.Add("business");
                else if (t.Name == "Loan") l.Add("loan");
                else if (t.Name == "TermDeposit") l.Add("cd");
            }
            return l;
        }

        public static IAccount AddAccount<TAccount>() where TAccount : IAccount
        {
            ConsoleUtil.Display(Properties.Resources.WhatAccountType);
            IAccount a;
            switch (ConsoleUtil.GetResponse(ListAccountTypes<TAccount>()))
            {
                case "checking":
                    a = new CheckingAccount(nextAccountId++);
                    break;
                case "business":
                    a = new BusinessAccount(nextAccountId++);
                    break;
                case "loan":
                    ConsoleUtil.WriteLine(Properties.Resources.WhatSizeLoan);
                    a = new Loan(nextAccountId++, ConsoleUtil.GetDollarAmount());
                    break;
                case "cd":
                    ConsoleUtil.WriteLine(Properties.Resources.WhatSizeCD);
                    a = new TermDeposit(nextAccountId++, ConsoleUtil.GetDollarAmount());
                    break;
                default:
                    Log.Error(Properties.Resources.ErrorInvalidProgramFlow);
                    throw new Exception("Bank: Invalid program flow.");
            }
            ConsoleUtil.WriteLine(Properties.Resources.GiveAccountName);
            var name = ConsoleUtil.GetString();
            while (currentCustomer.HasAccount(name))
            {
                ConsoleUtil.Display(Properties.Resources.AccountNameUnavailable);
                name = ConsoleUtil.GetString();
            }
            a.Name = name;
            currentCustomer.AddAccount(a);
            return a;
        }

        public static bool CloseAccount(IAccount acc)
        {
            bool cannotCancel = false;
            if (acc is ITerm && !((ITerm)acc).IsMature)
            {
                if (!ConsoleUtil.GetConfirm(Properties.Resources.ConfirmClosePremature))
                    return false;
            }
            if (acc.Balance < 0)
            {
                ConsoleUtil.DisplayWait(Properties.Resources.CannotCloseFundsOwed);
                return false;
            }
            else if(acc.Balance > 0) // Help customer remove balance before closing account
            {
                bool exitRemoveBalance = false;
                while (!exitRemoveBalance)
                {
                    ConsoleUtil.WriteLine(string.Format(Properties.Resources.CloseRemoveFunds,acc.Balance.ToString()));
                    List<string> options = new List<string>();
                    if (acc is IChecking)
                        options.Add("withdraw");
                    if (acc is ITransfer)
                        options.Add("transfer");
                    options.Add("create checking");
                    if (acc is ITerm)
                    {
                        ((ITerm)acc).IsMature = true;
                        cannotCancel = true;
                    }
                    else options.Add("cancel");

                    switch (ConsoleUtil.GetResponse(options))
                    {
                        case "withdraw":
                            exitRemoveBalance = ConsoleUtil.PrintTransferResult(((IChecking)acc).Withdraw(acc.Balance));
                            break;
                        case "create checking":
                            options.Remove("create checking");
                            AddAccount<IChecking>();
                            break;
                        case "transfer":
                            if (currentCustomer.AccountCount() == 1)
                            {
                                ConsoleUtil.WriteLine(Properties.Resources.CreateCheckingNeeded);
                                AddAccount<IChecking>();
                            }
                            Transfer((ITransfer)acc);
                            break;
                        case "cancel":
                            return false;
                    }
                    if (acc.Balance == 0) exitRemoveBalance = true;
                }
            }
            if (currentCustomer.AccountCount() == 1)
            {
                ConsoleUtil.DisplayWait(Properties.Resources.DeletingFinalAccount);
                return RemoveCustomer();
            }
            ConsoleUtil.Display(Properties.Resources.AreYouSureDeleteAccount);
            if (cannotCancel || ConsoleUtil.GetConfirm())
            {
                currentCustomer.RemoveAccount(acc);
                return true;
            }
            else return false;
        }

        public static TAccount SelectAccount<TAccount>(string msg) where TAccount : IAccount
        {
            List<string> accNames = currentCustomer.GetAccountNames<TAccount>();
            if (accNames.Count() == 1) 
                return (TAccount)currentCustomer.GetAccount(accNames.First());
            if(!string.IsNullOrEmpty(msg)) 
                ConsoleUtil.WriteLine(msg);
            return (TAccount)currentCustomer.GetAccount(ConsoleUtil.GetResponse(accNames));
        }

        public static bool Transfer()
        {
            if (currentCustomer.AccountCount() < 2)
            {
                ConsoleUtil.DisplayWait(Properties.Resources.TransferRequiresTwoAccounts);
                return false;
            }
            ITransfer from = SelectAccount<ITransfer>(Properties.Resources.SelectAccountTransferFrom);
            return Transfer(from);
        }

        public static bool Transfer(ITransfer from)
        {
            if (currentCustomer.AccountCount() < 2)
            {
                ConsoleUtil.DisplayWait(Properties.Resources.TransferRequiresTwoAccounts);
                return false;
            }
            currentCustomer.RemoveAccount(from);
            IAccount to = SelectAccount<IAccount>(Properties.Resources.SelectAccountTransferTo);
            currentCustomer.AddAccount(from);

            ConsoleUtil.WriteLine(string.Format(Properties.Resources.TransferAmount,from.Name,to.Name));
            decimal amt = ConsoleUtil.GetDollarAmount();

            if (!(to is IDebt) && !(to is IChecking))
            {
                ConsoleUtil.DisplayWait(Properties.Resources.IncompatibleAccount);
                return false;
            }
            else if (to is IDebt && !(to is IChecking) && ((IDebt)to).AmountOwed() < amt) {
                if(!ConsoleUtil.GetConfirm(Properties.Resources.TransferNotMoreThanOwedConfirm))
                {
                    ConsoleUtil.DisplayWait(Properties.Resources.OperationCancelled);
                    return false;
                }
                amt = ((IDebt)to).AmountOwed();
            }

            if (ConsoleUtil.PrintTransferResult(from.TransferOut(amt)))
            {
                if(to is IChecking)
                {
                    ((IChecking)to).Deposit(amt);
                } else if(to is IDebt)
                {
                    ((IDebt)to).MakePayment(amt);
                }
                ConsoleUtil.WriteLine(string.Format(Properties.Resources.TransferSuccessful, amt, from.Name, to.Name));
                ConsoleUtil.DisplayWait(Properties.Resources.OperationComplete);
                return true;
            }
            ConsoleUtil.DisplayWait(Properties.Resources.TransferWithdrawalFailed);
            return false;
        }
        public static void CompoundInterest()
        {
            foreach (var customer in customers)
            {
                customer.GetAccounts<IAccount>().Where(a => a.Balance < 0).ToList()
                    .ForEach(a => a.CompoundInterest(InterestRate));
                customer.GetAccounts<ITerm>()
                    .ForEach(a => a.CompoundInterest(InterestRate));
                customer.GetAccounts<CheckingAccount>()
                    .ForEach(a => a.CompoundInterest(InterestRate));
                customer.GetAccounts<BusinessAccount>()
                    .ForEach(a => { if (a.Balance > 0) { a.CompoundInterest(InterestRate); } });
            }
        }

        public static void AgeAccounts(int months)
        {
            foreach (var customer in customers)
            {
                customer.GetAccounts<IAccount>().ForEach(a => a.LastUpdated = a.LastUpdated.AddMonths(-months));
            }
        }
    }
}
