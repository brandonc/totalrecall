using System;
using System.Collections.Generic;
using Mono.Options;
using TotalRecall.Configuration;

namespace TotalRecall.ConsoleApp
{
    class Program
    {
        class ConsoleConfig : IConfig
        {
            public string IndexFolder { get; set; }
            public bool Optimize { get; set; }
            public string SiteRootDirectory { get; set; }
        }

        static void Main(string[] args)
        {
            bool optimize = true;
            bool show_help = false;
            bool doquery = false;
            string indexFolder = ".totalrecall";
            string website = "";
            string query = "";

            var p = new OptionSet() {
		        { "o|optimize", "optimize index (default true)", v => { optimize = v != null; } },
                { "q|query", "Query index", v => { doquery = v != null; } },
		        { "h|?|help", "show this screen", v => { show_help = v != null; } },
		        { "i|index=", "Lucene index location (default .totalrecall)" , v => { indexFolder = v; } }
	        };

            List<string> extras;
            try
            {
                extras = p.Parse(args);

                if (!doquery && extras.Count > 0)
                    website = extras[0];
                else if (doquery && extras.Count > 0)
                    query = extras[0];
            } catch (OptionException e)
            {
                Console.Write("rekall: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `rekall -?' for more information.");
                return;
            }

            if ((String.IsNullOrEmpty(website) && String.IsNullOrEmpty(query)) || show_help)
            {
                PrintUsage();
                p.WriteOptionDescriptions(Console.Out);
                return;
            }

            if (doquery)
            {
                var se = new SearchEngine();
                int index = 1;
                foreach (var hit in se.Search(query, 9))
                {
                    Console.WriteLine("{0}. {1}", index, hit.Title);
                    Console.WriteLine("   {0}", hit.Url);
                    Console.WriteLine();
                    index++;
                }
            } else
            {
                var crawler = new SiteCrawler(website, new ConsoleConfig()
                {
                    IndexFolder = indexFolder,
                    Optimize = optimize
                }, new ConsoleLogWrapper());

                crawler.Crawl();
            }

#if DEBUG
            Console.WriteLine();
            Console.Write("Press any key to continue");
            Console.ReadKey();
#endif
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage: rekall [url]");
        }
    }
}
