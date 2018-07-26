using System;
using System.Collections.Generic;

namespace BnrScrapperLogic
{
    public interface IRateRepository
    {
        void InsertBatchEuroRonRate(List<EuroRonRate> rates);
        List<RoborHistoric> GetRobors(DateTime from, DateTime to);
        RoborHistoric GetRoborRecent();
        void InsertBatch(List<RoborHistoric> historics);
    }
}