using System;
using System.Collections.Generic;

namespace BnrScrapperLogic.Loan
{
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


        public List<LoanTransaction> GenerateLoanTransactions()
        {

            var monthlyPayment=MonthlyPayment() ;
            var loanAmmount = Ammount;
            var lst = new List<LoanTransaction>();
            for (int i = 1; i <= Months; i++)
            {
                
                var dobandaVal = Math.Round(Ammount * Rate / 100 / 12, 2);
                var capital = monthlyPayment - dobandaVal;
                loanAmmount -= capital;
                var obj = new LoanTransaction(RateDateOfPayment.AddMonths(i), dobandaVal, capital, monthlyPayment, Math.Round(loanAmmount,2));
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