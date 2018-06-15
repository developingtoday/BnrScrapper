using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace BnrScrapper
{
    public class BnrRateScrapper
    {
        private static readonly object locker = new object();


        private readonly HtmlDocument document;

        public BnrRateScrapper(Stream input)
        {
            document = new HtmlDocument();
            document.Load(input);
        }

        public List<RoborHistoric> GetValues()
        {
            return ParseHtmlParallel(document).OrderByDescending(a => a.Data).ToList();
        }

        private static List<RoborHistoric> ParseHtmlParallel(HtmlDocument doc)
        {
            //var list=new List<RoborHistoric>();
            var startIndex = 3;
            var rowIndex = 3;
            var len = doc.DocumentNode?.SelectNodes($"//*[@id=\"GridView1\"]/tr").Count;
            var arr = new RoborHistoric[len.GetValueOrDefault() + rowIndex];
            var grid = doc.DocumentNode?.SelectSingleNode($"//*[@id=\"GridView1\"]");

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.For(startIndex, len.GetValueOrDefault(), new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, a =>
            {
                var roborStuff = ExtractFromTemplate(grid, a);
                arr[a] = roborStuff;
            });
            stopwatch.Stop();
            Console.WriteLine($"Processing finished - took {stopwatch.ElapsedMilliseconds} ms");

            return arr.Where(a => a != null).ToList();

        }

        private static RoborHistoric ExtractFromTemplate(HtmlNode grid, int rowIndex)
        {
            var tr = grid.ChildNodes.Where(e => e.Name == "tr").ToArray()[rowIndex];

            var date = tr.ChildNodes[1].InnerHtml;
            var robid3m = tr.ChildNodes[6].InnerHtml;
            var robid6m = tr.ChildNodes[7].InnerHtml;
            var robid9m = tr.ChildNodes[8].InnerHtml;
            var robid12m = tr.ChildNodes[9].InnerHtml;

            var robor3m = tr.ChildNodes[14].InnerHtml;
            var robor6m = tr.ChildNodes[15].InnerHtml;
            var robor9m = tr.ChildNodes[16].InnerHtml;
            var robor12m = tr.ChildNodes[17].InnerHtml;

            var roborStuff = new RoborHistoric()
            {
                Data = DateTime.Parse(date),
                Robid12M = decimal.Parse(robid12m, CultureInfo.InvariantCulture),
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