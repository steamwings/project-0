using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public interface IAccount
    {
        public int ID { get; }
        public string Name { get; set; }
        public double Balance { get; }
        public void DisplayInfo();

    }

    public interface IDebt : IAccount
    {
        public bool MakePayment(int payment);
        public double AmountOwed();
    }
}
