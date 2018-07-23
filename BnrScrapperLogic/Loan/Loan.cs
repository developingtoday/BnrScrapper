using System;

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

       

    }
}