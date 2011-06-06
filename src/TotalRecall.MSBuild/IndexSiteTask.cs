using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Build.Framework;
using TotalRecall.Configuration;

namespace TotalRecall.MSBuild
{
    public class IndexSiteTask : Microsoft.Build.Utilities.Task, IConfig
    {
        [Required]
        public string PublishedWebsiteUrl { get; set; }

        [Required]
        public string IndexFolder { get; set; }

        public string SiteRootDirectory { get; set; }

        public bool Optimize { get; set; }

        public override bool Execute()
        {
            SiteCrawler crawler = new SiteCrawler(this.PublishedWebsiteUrl, this);
            crawler.Crawl();

            return true;
        }
    }
}
