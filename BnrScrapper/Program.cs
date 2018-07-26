using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using BnrScrapperLogic;
using BnrScrapperLogic.Loan;
using HtmlAgilityPack;

namespace BnrScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            var loan = new Loan(254006.51, new DateTime(2018, 5, 6), 6.22, 226);
            var lst = loan.GenerateLoanTransactions();

            lst.ForEach(Console.WriteLine);
            Console.WriteLine("E gata smecheria");
            Console.ReadKey();
        }
 
    }
}
