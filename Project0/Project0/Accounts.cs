using System;
using System.Collections.Generic;

namespace Project0
{
    public abstract class Account : IAccount
    {
        public int ID { get; }
        public string Name { get; set; }
        //protected DollarAmount Balance { get; protected set; } = new DollarAmount();
        public decimal Balance { get; set; }
        public DateTime Updated { get; set; }
        public List<Transaction> Transactions { get; } = new List<Transaction>();
        
        public bool CompoundInterest(decimal rate)
        {
            int months = DateTime.Now.MonthsBefore(Updated);
            if (months < 1) return false;

            return true;
        }

        protected Account(int id)
        {
            ID = id;
            Transactions.Add(new Transaction(TransactionType.OpenAccount));
        }

        public virtual string Info()
        {
            return $"Account Name:{Name}\nBalance: {Balance}\n";
        }
    }

    public class CheckingAccount : Account, IChecking
    {
        public CheckingAccount(int id) : base(id) { }

        public virtual void Deposit(decimal amount)
        {
            Transactions.Add(new Transaction(TransactionType.Deposit, amount));
            Balance += amount;
        }

        public TransferResult TransferOut(decimal amount)
        {
            return Withdraw(amount);
        }

        public virtual TransferResult Withdraw(decimal amount)
        {
            TransferResult res;
            if (Balance < amount) res = TransferResult.InsufficientFunds;
            else
            {
                Balance -= amount;
                res = TransferResult.SuccessNoBorrow;
            }
            Transactions.Add(new Transaction(TransactionType.TransferOutOf, amount, res));
            return res;
        }
    }

    public class BusinessAccount : CheckingAccount, IDebt
    {
        public BusinessAccount(int id) : base(id){}

        public bool MakePayment(decimal payment)
        {
            Deposit(payment);
            return true;
        }

        public decimal AmountOwed()
        {
            if (Balance >= 0) return 0;
            else return (0 - Balance);
        }

        public override TransferResult Withdraw(decimal amount)
        {
            TransferResult res = TransferResult.SuccessNoBorrow;
            decimal diff = Balance - amount;
            if(diff < 0)
            {
                Balance -= (diff * Bank.InterestRate);
                res = TransferResult.SuccessBorrowing;
            }
            Balance -= amount;
            Transactions.Add(new Transaction(TransactionType.TransferOutOf, amount, res));
            return res;
        }
    }

    public class Loan : Account, IDebt
    {
        public Loan(int id, decimal amount) : base(id)
        {
            // Negative balance indicates amount owed
            Balance -= amount;
        }

        public decimal AmountOwed()
        {
            return -Balance;
        }

        public bool MakePayment(decimal amount)
        {
            if(Balance < 0 && (Balance + amount) <= 0)
            {
                Balance += amount;
                Transactions.Add(new Transaction(TransactionType.Payment, amount));
                return true;
            }
            return false;
        }
    }

    public class TermDeposit : Account, ITerm
    {
        public bool IsMature { get; set; } = false;
        public TermDeposit(int id, decimal amount) : base(id)
        {
            Balance += amount;
        }
        public override string Info()
        {
            return base.Info() + (IsMature ? "(Maturation reached.)" : "(Maturation date has not been reached.)");
        }

        public virtual TransferResult TransferOut(decimal amt)
        {
            TransferResult res;
            if (!IsMature)
            {
                res = TransferResult.ImmatureFunds;
            } else if(Balance < amt){
                res = TransferResult.InsufficientFunds;
            }
            else
            {
                Balance -= amt;
                res = TransferResult.SuccessNoBorrow;
            }
            Transactions.Add(new Transaction(TransactionType.TransferOutOf, amt, res));
            return res;
        }
    }
    
}
