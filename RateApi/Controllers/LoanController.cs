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
        private readonly IRateRepository _rateRepository;


        public LoanController(ILoanRepository loanRepository, IRateRepository rateRepository)
        {
            _loanRepository = loanRepository;
            _rateRepository = rateRepository;
        }

        [HttpGet("email/{email}")]
        public List<LoanInformation> GetInformation(string email)
        {
            return _loanRepository.GetLoansByEmail(email);
        }

        [HttpGet]
        public LoanInformation GetInformation(Guid loanId)
        {
            return _loanRepository.GetLoan(loanId);
        }

        [HttpGet("{loanId}/details")]
        public LoanDetails GetLoanDetails(Guid loanId)
        {
            var loanInformation = _loanRepository.GetLoan(loanId);
            var latestRates = _rateRepository.GetRoborRecent();
            var currentRate = loanInformation.BankRate + (double) latestRates.GetRate(loanInformation.BankMargin);
            var loan = new Loan(loanInformation.Ammount, loanInformation.RateDateOfPayment, currentRate,
                loanInformation.Months);
            var loanDetails = new LoanDetails(loanInformation);
            loanDetails.BankMarginRate = currentRate;
            loanDetails.Transactions = loan.GenerateLoanTransactions().Take(5).ToArray();
            return loanDetails;
        }

        [HttpPost]
        public bool SaveLoanInformation([FromBody]LoanInformation information)
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
