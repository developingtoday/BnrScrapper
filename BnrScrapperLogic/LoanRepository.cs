using System;
using System.Collections.Generic;
using BnrScrapperLogic.Loan;
using Newtonsoft.Json;

namespace BnrScrapperLogic
{
    public class LoanRepository:ILoanRepository
    {
        public bool SaveLoan(LoanInformation loan)
        {
            var conn = RedisConnection.Connection;
            var db = conn.GetDatabase(2);
            db.StringAppend(loan.Id.ToString(), JsonConvert.SerializeObject(loan));
            if (loan.SendEmail)
            {
                db.StringAppend("SendEmail", loan.Id.ToString());
            }

            db.StringAppend(loan.Email, loan.Id.ToString());
            return true;
        }

        public LoanInformation GetLoan(Guid loanId)
        {
            if(loanId==Guid.Empty) return new LoanInformation();
            var conn = RedisConnection.Connection;
            var db = conn.GetDatabase(2);
            var cmdResult = JsonConvert.DeserializeObject<LoanInformation>(db.StringGet(loanId.ToString()));
            return cmdResult;
        }

        public List<LoanInformation> GetLoansByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public bool DeleteLoan(Guid loan)
        {
            if (loan==Guid.Empty) return false;
            var conn = RedisConnection.Connection;
            var db = conn.GetDatabase(2);
            if (!db.KeyExists(loan.ToString())) return false;
            return db.KeyDelete(loan.ToString());
            
        }
    }
}