using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TotalRecall.Configuration
{
    public interface IConfig
    {
        string IndexFolder { get; set; }
        string SiteRootDirectory { get; set; }
        bool Optimize { get; set; }
    }
}
