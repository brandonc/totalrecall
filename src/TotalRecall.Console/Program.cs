using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mono.Options;

namespace TotalRecall.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            bool optimize = false;
            bool show_help = false;
            string indexFolder = ".totalrecall";
            string website = "";

            var p = new OptionSet() {
		        { "o|optimize", "optimize index (default true)", v => { optimize = v != null; } },
		        { "h|?|help", "show this screen", v => { show_help = v != null; } },
		        { "i|index=", "folder in which to store the Lucene index (default .totalrecall)" , v => { indexFolder = v; } }
	        };

            List<string> extras;
            try
            {
                extras = p.Parse(args);

                if (extras.Count > 0)
                    website = extras[0];
            } catch (OptionException e)
            {
                Console.Write("totalrecall: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try `totalrecall -?' for more information.");
                return;
            }

            if (String.IsNullOrEmpty(website) || show_help)
            {
                PrintUsage();
                p.WriteOptionDescriptions(Console.Out);
                return;
            }

            Console.WriteLine();
            Console.Write("Press any key to continue");
            Console.ReadKey();
        }

        static void PrintUsage()
        {
            Console.WriteLine("Usage: totalrecall [url]");
        }
    }
}
