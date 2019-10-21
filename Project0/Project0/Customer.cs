using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Project0
{
    public class Customer
    {
        public string Username { get; set; }
        private List<IAccount> accounts = new List<IAccount>();

        public Customer(string uname)
        {
            Username = uname;
        }

        public void AddAccount(IAccount a)
        {
            accounts.Add(a);
        }
        public void RemoveAccount(IAccount a)
        {
            accounts.Remove(a);
        }

        public void RemoveAllAccounts()
        {
            foreach(IAccount a in accounts)
            {
                RemoveAccount(a);
            }
        }
        public void DisplayAllAccounts()
        {
            foreach(IAccount a in accounts)
            {
                a.DisplayInfo();
            }
        }
        
        public bool FundsOwed()
        {
            foreach(IAccount a in accounts)
            {
                if (a.Balance < 0) return true;
            }
            return false;
        }

        public IEnumerable<CheckingAccount> GetCheckingAccounts()
        {
            return (IEnumerable<CheckingAccount>) accounts.Where(a => a is CheckingAccount);
        }

        public IEnumerable<string> GetAccountNames()
        {
            return from a in accounts select a.Name;
        }

        public IAccount GetAccount(string name)
        {
            return accounts.Where(a => a.Name == name).Single();
        }

    }
}
