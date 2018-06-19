using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace BnrScrapperLogic
{
    public class BnrRateScrapper<T>
    {
        


        private readonly HtmlDocument _document;
        private readonly IHtmlMap<T> _mapper;

        protected BnrRateScrapper(Stream input,IHtmlMap<T> mapper)
        {
            _document = new HtmlDocument();
            _document.Load(input);
            this._mapper = mapper;
        }

        public List<T> GetValues()
        {
            return ParseHtmlParallel(_document);
        }

        private  List<T> ParseHtmlParallel(HtmlDocument doc)
        {
            var startIndex = 3;
            var rowIndex = 3;
            var len = doc?.DocumentNode?.SelectNodes($"//*[@id=\"GridView1\"]/tr")?.Count;
            if (len.GetValueOrDefault() == 0)
            {
                return new List<T>();
            }
            var end = len.GetValueOrDefault() + 1;
            var arr = new T[len.GetValueOrDefault() + rowIndex];
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

        private  T ExtractFromTemplate(HtmlNode grid, int rowIndex)
        {
            var tr = grid.ChildNodes.Where(e => e.Name == "tr").ToArray()[rowIndex];
            return _mapper.Map(tr);

        }
    }

    public class BnrRoborScapper : BnrRateScrapper<RoborHistoric>
    {
        public BnrRoborScapper(Stream input) : base(input, new RoborMapper())
        {
        }
    }

    public class BnrEuroScapper : BnrRateScrapper<EuroRonRate>
    {
        public BnrEuroScapper(Stream input) : base(input, new EuroMapper())
        {
        }
    }
}