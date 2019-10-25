using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Project0
{
    public class InvalidDollarAmountException : Exception
    {

    }

    public class DollarAmount //TODO
    {

        private static Regex rg = new Regex(@"^\d{0,14}(.00)?$");
        private double _amount = 0;
        public void SetAmount(int val)
        {
            _amount = val;
        }

        public void Add(int x)
        {
            _amount += x;
        }

        public void Add(double x)
        {
            //if(!rg.IsMatch(resp))
        }

        public double Amount { get; private set; }

    }
}
