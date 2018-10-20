using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BnrScrapperLogic;
using BnrScrapperLogic.Loan;
using Microsoft.AspNetCore.Mvc;

namespace RateApi.Controllers
{
    [Route("api/[controller]")]
    public class LoanController:Controller
    {
        private readonly ILoanRepository _loanRepository;

        public LoanController(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        [HttpGet]
        public LoanInformation GetInformation(Guid loanId)
        {
            return _loanRepository.GetLoan(loanId);
        }

        [HttpPost]
        public bool SaveLoanInformation(LoanInformation information)
        {
            if (information.Id == Guid.Empty) information.Id = Guid.NewGuid();
            return _loanRepository.SaveLoan(information);
        }

        [HttpDelete]

        public bool DeleteLoanInformation(Guid loan)
        {
            return _loanRepository.DeleteLoan(loan);
        }


    }
}
