using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace TotalRecall.Configuration
{
    public class TotalRecallConfigurationSection : ConfigurationSection, IConfig
    {
        [ConfigurationProperty("indexfolder", DefaultValue = ".totalrecall")]
        public string IndexFolder
        {
            get
            {
                return (string)this["indexfolder"];
            }
            set
            {
                this["indexfolder"] = value;
            }
        }

        [ConfigurationProperty("siterootdirectory")]
        public string SiteRootDirectory
        {
            get
            {
                return (string)this["siterootdirectory"];
            }
            set
            {
                if (!value.StartsWith("/"))
                {
                    value = "/" + value;
                }
                this["siterootdirectory"] = value;
            }
        }

        [ConfigurationProperty("optimize", DefaultValue = false)]
        public bool Optimize
        {
            get
            {
                return (bool)this["optimize"];
            }
            set
            {
                this["optimize"] = value;
            }
        }
    }
}
