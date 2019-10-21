using System;

namespace Project0
{
    public abstract class BasicAccount : IAccount
    {
        public int ID { get; }
        public string Name { get; set; }
        //protected DollarAmount balance = new DollarAmount();
        public double Balance { get; protected set; }

        public BasicAccount(int id)
        {
            ID = id;
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Account Name:{Name}\nAccount ID: {ID}\n" +
                $"Balance: {Balance}\n");
        }
    }

    public class CheckingAccount : BasicAccount
    {
        public CheckingAccount(int id) : base(id) { }


        public virtual void Deposit(int amount)
        {
            Balance += amount;
        }

        public virtual bool Withdraw(int amount)
        {
            if (Balance < amount) return false;
            else
            {
                Balance -= amount;
                return true;
            }
        }
    }

    public class BusinessAccount : CheckingAccount, IDebt
    {
        public BusinessAccount(int id) : base(id){}

        public bool MakePayment(int payment)
        {
            Deposit(payment);
            return true;
        }

        public double AmountOwed()
        {
            if (Balance >= 0) return 0;
            else return (0 - Balance);
        }

        public override bool Withdraw(int amount)
        {
            double diff = Balance - amount;
            if(diff < 0)
            {
                Balance -= diff * Bank.InterestRate;
            }
            Balance -= amount;
            return true;
        }
    }

    public class Loan : BasicAccount, IDebt
    {
        public Loan(int id, int amount) : base(id)
        {
            // Negative balance indicates amount owed
            Balance -= amount;
        }

        public double AmountOwed()
        {
            return Balance;
        }

        public bool MakePayment(int amount)
        {
            if(Balance < 0)
            {
                Balance += amount;
                return true;
            }
            return false;
        }

    }

    public class TermDeposit : BasicAccount
    {
        public bool IsMature { get; } = false;
        public TermDeposit(int id, int amount) : base(id)
        {
            Balance += amount;
        }
        public override void DisplayInfo()
        {
            base.DisplayInfo();
            Console.WriteLine(IsMature ? "Maturation reached." : "Maturation date has not been reached.");
        }
    }
    
}
