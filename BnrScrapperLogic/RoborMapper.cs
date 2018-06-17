using System;
using System.Globalization;
using HtmlAgilityPack;

namespace BnrScrapperLogic
{
    public class RoborMapper : IHtmlMap<RoborHistoric>
    {
        public RoborHistoric Map(HtmlNode tr)
        {
            var date = tr.ChildNodes[1].InnerHtml;
            var robid3M = tr.ChildNodes[6].InnerHtml;
            var robid6M = tr.ChildNodes[7].InnerHtml;
            var robid9M = tr.ChildNodes[8].InnerHtml;
            var robid12M = tr.ChildNodes[9].InnerHtml;

            var robor3M = tr.ChildNodes[14].InnerHtml;
            var robor6M = tr.ChildNodes[15].InnerHtml;
            var robor9M = tr.ChildNodes[16].InnerHtml;
            var robor12M = tr.ChildNodes[17].InnerHtml;

            var roborStuff = new RoborHistoric()
            {
                Data = DateTime.Parse(date),
                Robid12M = decimal.Parse(robid12M, CultureInfo.InvariantCulture),
                Robid3M = decimal.Parse(robid3M, CultureInfo.InvariantCulture),
                Robid6M = decimal.Parse(robid6M, CultureInfo.InvariantCulture),
                Robid9M = decimal.Parse(robid9M, CultureInfo.InvariantCulture),
                Robor12M = decimal.Parse(robor12M, CultureInfo.InvariantCulture),
                Robor3M = decimal.Parse(robor3M, CultureInfo.InvariantCulture),
                Robor6M = decimal.Parse(robor6M, CultureInfo.InvariantCulture),
                Robor9M = decimal.Parse(robor9M, CultureInfo.InvariantCulture)
            };
            return roborStuff;
        }
    }

    public class EuroMapper : IHtmlMap<EuroRonRate>
    {
        public EuroRonRate Map(HtmlNode node)
        {
            var date = node.ChildNodes[1].InnerHtml;
            var value = node.ChildNodes[2].InnerHtml;
            var rate = new EuroRonRate()
            {
                Data = DateTime.Parse(date),
                Valoare = decimal.Parse(value, CultureInfo.InvariantCulture)
            };
            return rate;
        }
    }
}