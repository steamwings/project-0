using System;
using System.Collections.Generic;
using System.Linq;

namespace Project0
{
    public class ConsoleUtil
    {

        public static int GetDollarAmount()
        {
            //var rg = new Regex(@"^\d{0,14}(.00)?$");
            var resp = Console.ReadLine();
            int amt;
            while (!int.TryParse(resp, out amt)/* || !rg.IsMatch(resp)*/)
            {
                Console.WriteLine(Properties.Resources.ValidAmount);
                resp = Console.ReadLine();
            }
            return amt;
        }

        public enum GetResponseOption{
            DisplayResponses,
            DoNotDisplayResponses
        }

        public static bool GetConfirm()
        {
            return GetResponse("yes", "no") == "yes";
        }

        public static string GetResponse(IEnumerable<string> r)
        {
            return GetResponse(r.ToList());
        }

        public static string GetResponse(params string[] vals){
            return GetResponse(new List<string>(vals));
        }

        public static string GetResponse(List<string> responses)
        {
            string s;
            do
            {
                Console.Write(Properties.Resources.PleaseEnter);
                var en = responses.GetEnumerator();
                en.MoveNext();
                Console.Write($"\"{en.Current}\"");
                while (en.MoveNext())
                {
                    Console.Write($" or \"{en.Current}\"");
                }
                Console.WriteLine();
                s = Console.ReadLine();
            }
            while (!responses.Contains(s));
            return s;
        }

        public static string GetString()
        {
            string s;
            do
            {
                s = Console.ReadLine();
            } while (string.IsNullOrEmpty(s));
            return s;
        }

    }
}
