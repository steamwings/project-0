namespace Project0
{
    public interface IAccount
    {
        //bool Active { get; } //TODO
        int ID { get; }
        string Name { get; set; }
        decimal Balance { get; set; }
        string Info();
    }

    // Accounts you can transfer from
    public interface ITransfer : IAccount
    {
        enum WithdrawalResult
        {
            SuccessNoBorrow,
            SuccessBorrowing,
            InsufficientFunds,
            ImmatureFunds
        }

        WithdrawalResult Withdraw(decimal amt);
        void Deposit(decimal amt);
    }

    // Accounts you can owe money on
    public interface IDebt : IAccount
    { 
        bool MakePayment(decimal payment);
        decimal AmountOwed();
    }
}
