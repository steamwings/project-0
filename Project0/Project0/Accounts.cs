using static Project0.ITransfer;

namespace Project0
{
    public abstract class Account : IAccount
    {
        public int ID { get; }
        public string Name { get; set; }
        //protected DollarAmount Balance { get; protected set; } = new DollarAmount();
        public decimal Balance { get; set; }

        public Account(int id)
        {
            ID = id;
        }

        public virtual string Info()
        {
            return $"Account Name:{Name}\nBalance: {Balance}\n";
        }
    }

    public abstract class BasicTransferAccount : Account, ITransfer
    {
        public BasicTransferAccount(int id) : base(id) { }
        public virtual void Deposit(decimal amount)
        {
            Balance += amount;
        }
        public abstract WithdrawalResult Withdraw(decimal amt);
    }

    public class CheckingAccount : BasicTransferAccount
    {
        public CheckingAccount(int id) : base(id) { }

        public override WithdrawalResult Withdraw(decimal amount)
        {
            if (Balance < amount) return WithdrawalResult.InsufficientFunds;
            else
            {
                Balance -= amount;
                return WithdrawalResult.SuccessNoBorrow;
            }
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

        public override WithdrawalResult Withdraw(decimal amount)
        {
            WithdrawalResult res = WithdrawalResult.SuccessNoBorrow;
            decimal diff = Balance - amount;
            if(diff < 0)
            {
                Balance -= (diff * Bank.InterestRate);
                res = WithdrawalResult.SuccessBorrowing;
            }
            Balance -= amount;
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
            return Balance;
        }

        public bool MakePayment(decimal amount)
        {
            if(Balance < 0 && (Balance + amount) <= 0 )
            {
                Balance += amount;
                return true;
            }
            return false;
        }
    }

    public class TermDeposit : BasicTransferAccount
    {
        public bool IsMature { get; } = false;
        public TermDeposit(int id, decimal amount) : base(id)
        {
            Balance += amount;
        }
        public override string Info()
        {
            return base.Info() + (IsMature ? "Maturation reached." : "Maturation date has not been reached.");
        }

        public override WithdrawalResult Withdraw(decimal amt)
        {
            if (!IsMature)
            {
                return WithdrawalResult.ImmatureFunds;
            } else if(Balance < amt){
                return WithdrawalResult.InsufficientFunds;
            }
            Balance -= amt;
            return WithdrawalResult.SuccessNoBorrow;
        }
    }
    
}
