using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using BnrScrapperLogic;
using HtmlAgilityPack;

namespace BnrScrapper
{
    class Program
    {
        static void Main(string[] args)
        {
            double imprumut = 254006.51;
            int luni = 226;
            double dobanda = 6.22;
            var ratalunara = Math.Round(PMT(dobanda, luni, imprumut),2);
            var lst = new List<object>();
            var date = new DateTime(2018, 5, 6);
            var valoareAnticipata = 20000;
            for (int i=1; i<=luni; i++)
            {
                if (i <= 0)
                {
                    //after a year simulate what happens anticipate payment;
                    imprumut -= valoareAnticipata;
                    Console.WriteLine("Rambursare anticipata");
                    ratalunara = Math.Round(PMT(dobanda, luni, imprumut),2);

                }
                var dobandaVal = Math.Round(imprumut * dobanda / 100 / 12,2);
                var capital = ratalunara - dobandaVal;
                imprumut -= capital;
                var obj = new
                {
                    Data=date.AddMonths(i).ToShortDateString(),
                    dobanda= dobandaVal,
                    capital,
                    imprumut=Math.Round(imprumut,2),
                    ratalunara
                };
                if (imprumut <= 0)
                {
                    break;
                }
                Console.WriteLine(obj);
                
                lst.Add(obj);
                
            }
            Console.WriteLine($"Years:{lst.Count / 12}");
           
            Console.WriteLine("E gata smecheria");
            Console.ReadKey();
        }
        public static double PMT(double yearlyInterestRate, int totalNumberOfMonths, double loanAmount)
        {
            var rate = yearlyInterestRate / 100 / 12;
            var denominator = Math.Pow((1 + rate), totalNumberOfMonths) - 1;
            return (rate + (rate / denominator)) * loanAmount;
        }
    }

    public class Loan
    {
        public Loan(decimal ammount, DateTime rateDateOfPayment, double rate, int months)
        {
            Ammount = ammount;
            RateDateOfPayment = rateDateOfPayment;
            Rate = rate;
            Months = months;
        }

        public decimal Ammount { get; private set; }

        public DateTime RateDateOfPayment { get; set; }

        public double Rate { get; private set; }

        public int Months { get; private set; }

    }

    public class LoanTransaction
    {
        public DateTime Date { get; private set; }

        public double InterestValue { get; private set; }

        public double CapitalValue { get; private set; }

        public double MonthlyPayment { get; private set; }

        public double LoanValue { get; private set; }
    }

    public class LoanReturnService
    {

    }
}
