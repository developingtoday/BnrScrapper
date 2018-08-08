using System;

namespace BnrScrapperLogic
{
    public class RoborHistoric
    {

        public DateTime Data { get; set; }

        public decimal Robid3M { get; set; }
        public decimal Robid6M { get; set; }
        public decimal Robid9M { get; set; }
        public decimal Robid12M { get; set; }

        public decimal Robor3M { get; set; }
        public decimal Robor6M { get; set; }
        public decimal Robor9M { get; set; }
        public decimal Robor12M { get; set; }

        public override string ToString()
        {
            return $"{nameof(Data)}: {Data}, {nameof(Robid3M)}: {Robid3M}, {nameof(Robid6M)}: {Robid6M}, {nameof(Robid9M)}: {Robid9M}, {nameof(Robid12M)}: {Robid12M}, {nameof(Robor3M)}: {Robor3M}, {nameof(Robor6M)}: {Robor6M}, {nameof(Robor9M)}: {Robor9M}, {nameof(Robor12M)}: {Robor12M}";
        }
    }

    public class EuroRonRate
    {
        public DateTime Data { get; set; }
        public decimal Valoare { get; set; }

        public override string ToString()
        {
            return $"{nameof(Data)}: {Data}, {nameof(Valoare)}: {Valoare}";
        }
    }

    public class RatePushNotification
    {
        public string Data { get; set; }
        public decimal Robor3M { get; set; }
        public decimal Robid3M { get; set; }

        public decimal EuroRonRate { get; set; }

        public decimal Delta { get; set; }

        public override string ToString()
        {
            return $"{nameof(Data)}: {Data}, {nameof(Robor3M)}: {Robor3M}, {nameof(Robid3M)}: {Robid3M}, {nameof(EuroRonRate)}: {EuroRonRate}";
        }

        public bool SendRoborPush => Robid3M != 0 && this.Robor3M != 0;
    }
}