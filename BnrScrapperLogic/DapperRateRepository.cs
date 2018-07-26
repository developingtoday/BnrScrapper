using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using Dapper;


namespace BnrScrapperLogic
{
    public class DapperRateRepository:IRateRepository
    {
        private readonly string _connString;

        public DapperRateRepository(string connString)
        {
            _connString = connString;
        }


        public void InsertBatchEuroRonRate(List<EuroRonRate> rates)
        {
            throw new NotImplementedException();
        }

        public List<RoborHistoric> GetRobors(DateTime @from, DateTime to)
        {
            using (IDbConnection connection=new SqlConnection(_connString))
            {

                var result = connection.Query<RoborHistoric>(
                    @"select 
                    r.[RateDate] as Data
                  ,r.[Robid3M]
                  ,r.[Robid6M]
                  ,r.[Robid9M]
                  ,r.[Robid12M]
                  ,r.[Robor3M]
                  ,r.[Robor6M]
                  ,r.[Robor9M]
                  ,r.[Robor12M]
                    from dbo.Rates r where @from<=r.RateDate and r.RateDate<=@to", new{ From=from.Date, To=to.Date});
                
                return result.ToList();
            }
        }

        public RoborHistoric GetRoborRecent()
        {
            using (IDbConnection connection = new SqlConnection(_connString))
            {

                var result = connection.Query<RoborHistoric>(
                    @"select 
                    top 1
                    r.[RateDate] as Data
                  ,r.[Robid3M]
                  ,r.[Robid6M]
                  ,r.[Robid9M]
                  ,r.[Robid12M]
                  ,r.[Robor3M]
                  ,r.[Robor6M]
                  ,r.[Robor9M]
                  ,r.[Robor12M]
                 from dbo.Rates r 
                order by r.RateDate desc");
                return result.FirstOrDefault();
            }
        }

        public void InsertBatch(List<RoborHistoric> historics)
        {
            throw new NotImplementedException();
        }
    }

    
}
