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
            var loan = new Loan(254006.51, new DateTime(2018, 5, 6), 6.22, 226);
            var lst = LoanReturnService.GenerateLoanTransactions(loan);

            lst.ForEach(Console.WriteLine);
            Console.WriteLine("E gata smecheria");
            Console.ReadKey();
        }
 
    }

    public class Loan
    {
        public Loan(double ammount, DateTime rateDateOfPayment, double rate, int months)
        {
            Ammount = ammount;
            RateDateOfPayment = rateDateOfPayment;
            Rate = rate;
            Months = months;
        }

        public double Ammount { get; private set; }

        public DateTime RateDateOfPayment { get; set; }

        public double Rate { get; private set; }

        public int Months { get; private set; }

        private static double PMT(double yearlyInterestRate, int totalNumberOfMonths, double loanAmount)
        {
            var rate = yearlyInterestRate / 100 / 12;
            var denominator = Math.Pow((1 + rate), totalNumberOfMonths) - 1;
            return (rate + (rate / denominator)) * loanAmount;
        }

        public double MonthlyPayment()
        {
            return Math.Round(PMT(Rate, Months, Ammount), 2);
        }

       

    }

    public class LoanTransaction
    {
        public LoanTransaction(DateTime date, double interestValue, double capitalValue, double monthlyPayment, double loanValue)
        {
            Date = date;
            InterestValue = interestValue;
            CapitalValue = capitalValue;
            MonthlyPayment = monthlyPayment;
            LoanValue = loanValue;
        }

        public DateTime Date { get; private set; }

        public double InterestValue { get; private set; }

        public double CapitalValue { get; private set; }

        public double MonthlyPayment { get; private set; }

        public double LoanValue { get; private set; }

        public override string ToString()
        {
            return $"{nameof(Date)}:{Date}, {nameof(InterestValue)}:{InterestValue}, {nameof(CapitalValue)}:{CapitalValue}, {nameof(MonthlyPayment)}:{MonthlyPayment}, {nameof(LoanValue)}:{LoanValue}";
        }
    }

    public class LoanReturnService
    {
        public static List<LoanTransaction> GenerateLoanTransactions(Loan loan)
        {
            var monthlyPayment=loan.MonthlyPayment() ;
            var loanAmmount = loan.Ammount;
            var lst = new List<LoanTransaction>();
            for (int i = 1; i <= loan.Months; i++)
            {
                
                var dobandaVal = Math.Round(loan.Ammount * loan.Rate / 100 / 12, 2);
                var capital = monthlyPayment - dobandaVal;
                loanAmmount -= capital;
                var obj = new LoanTransaction(loan.RateDateOfPayment.AddMonths(i), dobandaVal, capital, monthlyPayment, Math.Round(loanAmmount,2));
                if (loanAmmount <= 0)
                {
                    break;
                }
                lst.Add(obj);

            }
            return lst;
        }

       
    }
}
