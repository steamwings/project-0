using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public class BasicAccount : IAccount
    {
        public int ID { get; }
        public string Name { get; set; }
        protected double balance = 0;
        
        public BasicAccount(int id)
        {
            ID = id;
        }

        public void Deposit(int amount)
        {
            balance += amount;
        }

        public virtual bool Withdraw(int amount)
        {
            if (balance < amount) return false;
            else {
                balance -= amount;
                return true;
            }
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Account Name:{Name}\nAccount ID: {ID}\n" +
                $"Balance: {balance}\n");
        }
    }

   public class BusinessAccount : BasicAccount
    {
        public BusinessAccount(int id) : base(id){}

        public override bool Withdraw(int amount)
        {
            balance -= amount;
            return true;
        }
    }

    public class Loan : BasicAccount
    {
        public Loan(int id, int amount) : base(id)
        {
            // Negative balance indicates amount owed
            balance -= amount;
        }

        public override bool Withdraw(int amount)
        {
            Console.WriteLine(Properties.Resources.LoanWithdraw);
            return false;
        }
    }

    public class TermDeposit : BasicAccount
    {
        public TermDeposit(int id, int amount) : base(id)
        {
            balance += amount;
        }

        public override bool Withdraw(int amount)
        {
            Console.WriteLine(Properties.Resources.TermDepositWithdraw);
            return false;
        }
    }
    
}
