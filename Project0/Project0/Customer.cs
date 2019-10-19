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

        public bool Withdraw(int id, int amount)
        {
            return accounts.Where(a => a.ID == id).().Withdraw(amount);
            
        }
        public bool Withdraw(string name, int amount)
        {
            return accounts.Where(a => a.Name == name).Single().Withdraw(amount);
        }
        

    }
}
