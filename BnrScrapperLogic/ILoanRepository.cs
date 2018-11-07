using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BnrScrapperLogic.Loan;

namespace BnrScrapperLogic
{
    public interface ILoanRepository
    {
        bool SaveLoan(LoanInformation loan);
        LoanInformation GetLoan(Guid loanId);
        List<LoanInformation> GetLoansByEmail(string email);
        bool DeleteLoan(Guid loan);
    }
}
