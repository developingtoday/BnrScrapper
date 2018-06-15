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
            var startIndex = 3;
            var rowIndex = 3;
            var len = doc.DocumentNode?.SelectNodes($"//*[@id=\"GridView1\"]/tr").Count;
            if (len.GetValueOrDefault() == 0)
            {
                return new List<RoborHistoric>();
            }
            var end = len.GetValueOrDefault() + 1;
            var arr = new RoborHistoric[len.GetValueOrDefault() + rowIndex];
            var grid = doc.DocumentNode?.SelectSingleNode($"//*[@id=\"GridView1\"]");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            Parallel.For(startIndex, end, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, a =>
            {
                var roborStuff = ExtractFromTemplate(grid, a-1);
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
}