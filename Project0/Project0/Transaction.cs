using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Project0
{
    public enum TransactionType
    {
        Deposit,
        Withdraw,
        TransferOutOf,
        TransferInto,
        Payment,
        OpenAccount,
        CloseAccount
    }
    public class Transaction
    {
        public readonly TransactionType Type;
        public readonly TransferResult? TransferResult = null;
        public readonly decimal? Amount = null;
        public readonly DateTime Time = DateTime.UtcNow;
        public Transaction(TransactionType t)
        {
            Type = t;
        }
        public Transaction(TransactionType t, decimal amt)
        {
            Type = t;
            Amount = amt;
        }

        public Transaction(TransactionType t, decimal amt, TransferResult tr)
        {
            Type = t;
            TransferResult = tr;
            Amount = amt;
        }


        public (string time, string transaction, string amt, string transfer) Values()
        {
            return (Time.ToString(), Enum.GetName(typeof(TransactionType), Type), Amount.HasValue ? $"${Amount}" : "",
                TransferResult.HasValue ? $"{Enum.GetName(typeof(TransferResult), TransferResult.Value)}" : "");
        }

        public override string ToString()
        {
            var vals = Values();
            return $"{vals.time}\t{vals.transaction}\t{vals.amt}\t{vals.transfer}";
        }

        public static string FormatTransactions(List<Transaction> list)
        {
            List<ValueTuple<string,string,string,string>> vals = list.Select(t => t.Values()).ToList();
            List<int> offsets = new List<int>(4);
            offsets.Add(4+vals.Select(v => v.Item1).Max(s => s.Length));
            offsets.Add(4+vals.Select(v => v.Item2).Max(s => s.Length));
            offsets.Add(4+vals.Select(v => v.Item3).Max(s => s.Length));
            offsets.Add(4+vals.Select(v => v.Item4).Max(s => s.Length));
            return string.Join(Environment.NewLine, vals.Select(
                vt => vt.Item1.PadRight(offsets[0]) + vt.Item2.PadRight(offsets[1]) 
                + vt.Item3.PadRight(offsets[2]) + vt.Item4.PadRight(offsets[3])
            ));
        }
    }
}
