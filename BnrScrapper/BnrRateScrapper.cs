using System;
using System.Collections.Concurrent;
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
            //var list=new List<RoborHistoric>();
            var startIndex = 3;
            var rowIndex = 3;
            var len = doc.DocumentNode?.SelectNodes($"//*[@id=\"GridView1\"]/tr").Count;
            //rowIndex += len.GetValueOrDefault();
            //for (;;)
            //{
            //    var check = doc.DocumentNode?.SelectNodes($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[1]");
            //    if (check == null)
            //    {
            //        break;
            //    }

            //    rowIndex++;
            //    len++;
            //}
            //var listConc = new ConcurrentBag<RoborHistoric>();
            var arr = new RoborHistoric[len.GetValueOrDefault()+rowIndex];
            Parallel.For(startIndex, len.GetValueOrDefault(), new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, a =>
            {
                var roborStuff = ExtractFromTemplate(doc, a);
                //lock (locker)
                //{
                //    list.Add(roborStuff);
                //}
                //listConc.Add(roborStuff);
                arr[a] = roborStuff;
                //arr[a- startIndex] = roborStuff;
            });
            
            return arr.Where(a=>a!=null).ToList();

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
            var date = doc.DocumentNode?.SelectSingleNode($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[1]")?.InnerText;
            var robid3m = doc.DocumentNode.SelectSingleNode($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[6]")?.InnerText;
            var robid6m = doc.DocumentNode.SelectSingleNode($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[7]")?.InnerText;
            var robid9m = doc.DocumentNode.SelectSingleNode($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[8]")?.InnerText;
            var robid12m = doc.DocumentNode.SelectSingleNode($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[9]")?.InnerText;

            var robor3m = doc.DocumentNode.SelectSingleNode($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[14]")?.InnerText;
            var robor6m = doc.DocumentNode.SelectSingleNode($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[15]")?.InnerText;
            var robor9m = doc.DocumentNode.SelectSingleNode($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[16]")?.InnerText;
            var robor12m = doc.DocumentNode.SelectSingleNode($"//*[@id=\"GridView1\"]/tr[{rowIndex}]/td[17]")?.InnerText;

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