using System;
using System.Collections.Generic;
using System.Text;
using NCrawler;
using NCrawler.Services;
using System.Text.RegularExpressions;
using NCrawler.HtmlProcessor;
using TotalRecall.Configuration;
using System.IO;
using System.Configuration;
using log4net;
using log4net.Repository.Hierarchy;
using log4net.Appender;
using log4net.Layout;
using log4net.Core;

namespace TotalRecall
{
    public class SiteCrawler
    {
        public string WebsiteUrl { get; set; }
        public IConfig Config { get; set; }

        public void Crawl()
        {
            ILog log = LogManager.GetLogger(typeof(SiteCrawler));

            using (Crawler c = new Crawler(new Uri(this.WebsiteUrl), new HtmlDocumentProcessor(), new DocumentIndexStep(this.Config, log)))
            {
                c.AdhereToRobotRules = true;
                c.MaximumThreadCount = System.Environment.ProcessorCount * 2;
                c.ExcludeFilter = new[] {
                    new NCrawler.Services.RegexFilter(new Regex(@"(\.jpg|\.css|\.js|\.gif|\.jpeg|\.png|\.ico)"))
                };
                c.Crawl();
            }
        }

        public SiteCrawler(string websiteUrl)
        {
            WebsiteUrl = websiteUrl;
        }

        public SiteCrawler(string websiteUrl, IConfig config)
        {
            WebsiteUrl = websiteUrl;
            Config = config;
        }

        static SiteCrawler()
        {
            Hierarchy hierarchy = (Hierarchy)LogManager.GetRepository();
            TraceAppender tracer = new TraceAppender();
            PatternLayout patternLayout = new PatternLayout();

            patternLayout.ConversionPattern = "%timestamp %-5level %logger{2} - %message%newline";
            patternLayout.ActivateOptions();

            tracer.Layout = patternLayout;
            tracer.ActivateOptions();
            hierarchy.Root.AddAppender(tracer);

            RollingFileAppender roller = new RollingFileAppender();
            roller.Layout = patternLayout;
            roller.AppendToFile = true;
            roller.RollingStyle = RollingFileAppender.RollingMode.Size;
            roller.MaxSizeRollBackups = 4;
            roller.MaximumFileSize = "100KB";
            roller.StaticLogFileName = true;
            roller.File = "crawler.log";
            roller.ActivateOptions();
            hierarchy.Root.AddAppender(roller);

            hierarchy.Root.Level = Level.All;
            hierarchy.Configured = true;
        }
    }
}
