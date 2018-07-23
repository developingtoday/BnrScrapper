using System;
using System.Collections.Generic;

namespace BnrScrapperLogic.Loan
{
    public static class LoanReturnService
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