using System;
using System.Linq;
using System.Text;
using BnrScrapperLogic.Loan;

namespace BnrScrapperLogic
{
    public interface ILoanRepository
    {
        bool SaveLoan(LoanInformation loan);
        LoanInformation GetLoan(Guid loanId);

        bool DeleteLoan(Guid loan);
    }
}
