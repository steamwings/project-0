using System;
using System.Collections.Generic;
using System.Text;

namespace Project0
{
    public static class Extensions
    {
        //https://github.com/markwhitaker/MonthDiff
        public static int GetTotalMonthsFrom(this DateTime dt1, DateTime dt2)
        {
            var earlyDate = (dt1 > dt2) ? dt2.Date : dt1.Date;
            var lateDate = (dt1 > dt2) ? dt1.Date : dt2.Date;

            // Start with 1 month's difference and keep incrementing
            // until we overshoot the late date
            int monthsDiff = 1;
            while (earlyDate.AddMonths(monthsDiff) <= lateDate)
            {
                monthsDiff++;
            }

            return monthsDiff - 1;
        }

        public static int GetTotalMonthsFrom(this DateTimeOffset dt1, DateTimeOffset dt2)
        {
            return dt1.DateTime.GetTotalMonthsFrom(dt2.DateTime);
        }
    }
}
