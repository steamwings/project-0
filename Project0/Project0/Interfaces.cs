using System;
using System.Collections.Generic;

namespace Project0
{
    public interface IAccount
    {
        int ID { get; }
        string Name { get; set; }
        decimal Balance { get; set; }
        DateTime Updated { get; set; }
        List<Transaction> Transactions { get; }
        string Info();
    }

    public enum TransferResult
    {
        SuccessNoBorrow,
        SuccessBorrowing,
        InsufficientFunds,
        ImmatureFunds
    }

    // Accounts you can transfer from
    public interface ITransfer : IAccount
    {
        TransferResult TransferOut(decimal amt);
    }

    public interface IChecking : ITransfer
    {
        void Deposit(decimal amt);
        TransferResult Withdraw(decimal amt);
    }

    // Accounts you can owe money on
    public interface IDebt : IAccount
    { 
        bool MakePayment(decimal payment);
        decimal AmountOwed();
    }

    public interface ITerm :  ITransfer
    {
        bool IsMature { get; set; }
    }
}
