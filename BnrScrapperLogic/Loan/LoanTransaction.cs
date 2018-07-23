using System;

namespace BnrScrapperLogic.Loan
{
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
}