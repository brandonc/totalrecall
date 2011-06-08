using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TotalRecall.ConsoleApp
{
    class ConsoleLogWrapper : ILogWrapper
    {
        private readonly TextWriter tw;
        
        private void Internal(string level, string message) {
            tw.WriteLine("rekall: [{0}] {1}", level, message);
        }

        public void Debug(string message)
        {
            Internal("debug", message);
        }

        public void Warning(string message)
        {
            Internal("warning", message);
        }

        public void Info(string message)
        {
            Internal("info", message);
        }

        public void Error(string message)
        {
            Internal("error", message);
        }

        public ConsoleLogWrapper()
        {
            tw = Console.Out;
        }
    }
}
