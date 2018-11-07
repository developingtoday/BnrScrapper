using System;
using System.Collections.Generic;

namespace BnrScrapperLogic.Loan
{

    public class LoanInformation
    {
        public Guid Id { get; set; }

        public string Email { get; set; }

        public double BankRate { get; set; }

        public string BankMargin { get; set; }

        public bool SendEmail { get; set; }

        public decimal Ammount { get; set; }

        public DateTime RateDateOfPayment { get; set; }

        public int Months { get; set; }
        
    }
}