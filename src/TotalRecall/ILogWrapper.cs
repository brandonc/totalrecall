using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TotalRecall
{
    public interface ILogWrapper
    {
        void Debug(string message);
        void Warning(string message);
        void Info(string message);
        void Error(string message);
    }
}
