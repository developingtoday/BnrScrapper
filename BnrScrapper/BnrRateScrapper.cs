using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace BnrScrapper
{
   public class BnrRateScrapper
    {
        private static readonly object locker=new object();

        private readonly HtmlDocument document;

        public BnrRateScrapper(Stream input)
        {
            document = new HtmlDocument();
            document.Load(input);
        }

        public List<RoborHistoric> GetValues()
        {
            return ParseHtmlParallel(document).OrderByDescending(a=>a.Data).ToList();
        }
        public List<RoborHistoric> GetValuesNonParallel()
        {
            return ParseHtml(document).OrderByDescending(a => a.Data).ToList();
        }

        private static List<RoborHistoric> ParseHtmlParallel(HtmlDocument doc)
        {
            var list=new List<RoborHistoric>();
            var startIndex = 3;
            var rowIndex = 3;
            for (;;)
            {
                var check = doc.DocumentNode?.SelectNodes($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[1]");
                if (check == null)
                {
                    break;
                }
               
                rowIndex++;
            }
            Parallel.For(startIndex, rowIndex--, a =>
            {
                var roborStuff = ExtractFromTemplate(doc, a);
                lock (locker)
                {
                    list.Add(roborStuff);
                }
            });
            
            return list;

        }

        private static List<RoborHistoric> ParseHtml(HtmlDocument doc)
        {
            var list = new List<RoborHistoric>();
            var startIndex = 3;
            var rowIndex = 3;
            for (; ; )
            {
                var check = doc.DocumentNode?.SelectNodes($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[1]");
                if (check == null)
                {
                    break;
                }
                var roborStuff = ExtractFromTemplate(doc, rowIndex);
                list.Add(roborStuff);
                rowIndex++;
            }

            return list;

        }

        private static RoborHistoric ExtractFromTemplate(HtmlDocument doc,int rowIndex)
        {
            var date = doc.DocumentNode?.SelectNodes($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[1]")?.Nodes()?.Select(x => x.InnerText)
                ?.FirstOrDefault();
            var robid3m = doc.DocumentNode.SelectNodes($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[6]").Nodes().Select(x => x.InnerText)
                .FirstOrDefault();
            var robid6m = doc.DocumentNode.SelectNodes($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[7]").Nodes().Select(x => x.InnerText)
                .FirstOrDefault();
            var robid9m = doc.DocumentNode.SelectNodes($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[8]").Nodes().Select(x => x.InnerText)
                .FirstOrDefault();
            var robid12m = doc.DocumentNode.SelectNodes($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[9]").Nodes().Select(x => x.InnerText)
                .FirstOrDefault();

            var robor3m = doc.DocumentNode.SelectNodes($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[14]").Nodes().Select(x => x.InnerText)
                .FirstOrDefault();
            var robor6m = doc.DocumentNode.SelectNodes($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[15]").Nodes().Select(x => x.InnerText)
                .FirstOrDefault();
            var robor9m = doc.DocumentNode.SelectNodes($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[16]").Nodes().Select(x => x.InnerText)
                .FirstOrDefault();
            var robor12m = doc.DocumentNode.SelectNodes($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[17]").Nodes().Select(x => x.InnerText)
                .FirstOrDefault();

            var roborStuff = new RoborHistoric()
            {
                Data = DateTime.Parse(date),
                Robid12M = decimal.Parse(robid12m,CultureInfo.InvariantCulture),
                Robid3M = decimal.Parse(robid3m, CultureInfo.InvariantCulture),
                Robid6M = decimal.Parse(robid6m, CultureInfo.InvariantCulture),
                Robid9M = decimal.Parse(robid9m, CultureInfo.InvariantCulture),
                Robor12M = decimal.Parse(robor12m, CultureInfo.InvariantCulture),
                Robor3M = decimal.Parse(robor3m, CultureInfo.InvariantCulture),
                Robor6M = decimal.Parse(robor6m, CultureInfo.InvariantCulture),
                Robor9M = decimal.Parse(robor9m, CultureInfo.InvariantCulture)
            };
            return roborStuff;
        }
    }
}