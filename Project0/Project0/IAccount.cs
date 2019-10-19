using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public interface IAccount
    {
        public int ID { get; }
        public string Name { get; }
        public bool Withdraw(int amount);
        public void Deposit(int amount);
        public void DisplayInfo();

    }
}
