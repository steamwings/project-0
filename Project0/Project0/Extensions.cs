using System;

namespace Project0
{
    public static class Extensions
    {
        /// <summary>
        /// Extension method that answers: dt1 (this) is how many months before dt2?
        /// </summary>
        /// <param name="dt1"></param>
        /// <param name="dt2"></param>
        /// <returns>Number of months dt1 is before dt2. May be negative if dt2 &lt; dt1. </returns>
        public static int MonthsBefore(this DateTime dt1, DateTime dt2)
        {
            (DateTime early, DateTime late, bool dt2After) = dt2 > dt1 ? (dt1,dt2,true) : (dt2,dt1,false);
            DateTime tmp;
            int months = 1;
            while ((tmp = early.AddMonths(1)) <= late)
            {
                early = tmp;
                months++;
            }
            return (months-1)*(dt2After ? 1 : -1);
        }
    }
}
