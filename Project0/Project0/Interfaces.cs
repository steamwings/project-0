using System.Collections.Generic;

namespace Project0
{
    public interface IAccount
    {
        int ID { get; }
        string Name { get; set; }
        decimal Balance { get; set; }
        List<Transaction> Transactions { get; }
        string Info();
    }

    // Accounts you can transfer from
    public interface ITransfer : IAccount
    {
        enum TransferResult
        {
            SuccessNoBorrow,
            SuccessBorrowing,
            InsufficientFunds,
            ImmatureFunds
        }

        TransferResult TransferOut(decimal amt);
    }

    public interface IChecking : ITransfer
    {
        void Deposit(decimal amt);
        ITransfer.TransferResult Withdraw(decimal amt);
    }

    // Accounts you can owe money on
    public interface IDebt : IAccount
    { 
        bool MakePayment(decimal payment);
        decimal AmountOwed();
    }

    public interface ITerm :  ITransfer
    {
        bool IsMature { get; }
    }
}
