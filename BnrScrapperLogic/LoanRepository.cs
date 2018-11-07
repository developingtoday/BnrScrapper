using System;
using System.Collections.Generic;
using System.Linq;
using BnrScrapperLogic.Loan;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace BnrScrapperLogic
{
    public class LoanRepository:ILoanRepository
    {
        public bool SaveLoan(LoanInformation loan)
        {
            var conn = RedisConnection.Connection;
            var db = conn.GetDatabase(2);
            db.StringSet(loan.Id.ToString(), JsonConvert.SerializeObject(loan));
            db.ListRemove("SendEmail", loan.Id.ToString());
            db.ListRemove(loan.Email, loan.Id.ToString());
            if (loan.SendEmail)
            {
              db.ListLeftPush("SendEmail", loan.Id.ToString());
            }
            db.ListLeftPush(loan.Email, loan.Id.ToString());
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
            if (string.IsNullOrEmpty(email)) return new List<LoanInformation>(0);
            var conn = RedisConnection.Connection;
            var db = conn.GetDatabase(2);
            if (!db.KeyExists(email)) return new List<LoanInformation>(0);
            var loansByEmail = db.ListRange(email, 0, db.ListLength(email)).Select(a=>(RedisKey)a.ToString()).ToArray();
            var infos=db.StringGet(loansByEmail).Select(a=>JsonConvert.DeserializeObject<LoanInformation>(a)).ToList();
            return infos;
        }

        public bool DeleteLoan(Guid loan)
        {
            if (loan==Guid.Empty) return false;
            var conn = RedisConnection.Connection;
            var db = conn.GetDatabase(2);
            if (!db.KeyExists(loan.ToString())) return false;
            var info = GetLoan(loan);
            if (info != null)
            {
                db.ListRemove(info.Email, loan.ToString());
            }
            db.ListRemove("SendEmail", loan.ToString());

            return db.KeyDelete(loan.ToString());
            
        }
    }
}