using System;
using System.Collections.Generic;

namespace BnrScrapperLogic.Loan
{

    public class LoanInformation
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public double BankRate { get; set; }

        public BankMargin BankMargin { get; set; }

        public bool SendEmail { get; set; }

        public double Ammount { get; set; }

        public DateTime RateDateOfPayment { get; set; }

        public int Months { get; set; }
        
    }


    public class LoanDetails:LoanInformation
    {
        public LoanDetails(LoanInformation loanInformation)
        {
            this.Id = loanInformation.Id;
            this.Email = loanInformation.Email;
            this.BankRate = loanInformation.BankRate;
            this.BankMargin = loanInformation.BankMargin;
            this.SendEmail = loanInformation.SendEmail;
            this.Ammount = loanInformation.Ammount;
            this.RateDateOfPayment = loanInformation.RateDateOfPayment;
            this.Months = loanInformation.Months;
        }

        public double BankMarginRate { get; set; }
        public LoanTransaction[] Transactions { get; set; }

    }
}