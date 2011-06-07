using System;
using System.Configuration;
using System.IO;
using Lucene.Net.Analysis;
using Lucene.Net.Documents;
using Lucene.Net.Index;
using Lucene.Net.QueryParsers;
using Lucene.Net.Store;
using NCrawler.Interfaces;
using TotalRecall.Configuration;
using Lucene.Net.Search;
using NCrawler;
using Lucene.Net.Analysis.Standard;
using NCrawler.Events;

namespace TotalRecall
{
    class DocumentIndexStep : IPipelineStep
    {
        private ILogWrapper log;
        private IConfig config;
        private DocumentRepository repository;
        private bool bindevents = false;

        public DocumentIndexStep(IConfig config, ILogWrapper log)
        {
            this.config = config;
            this.log = log;

            this.repository = new DocumentRepository(
                new IndexWriter(
                    FSDirectory.Open(new DirectoryInfo(config.IndexFolder)),
                    new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29),
                    true,
                    IndexWriter.MaxFieldLength.UNLIMITED
                ),
                this.log
            );
        }

        private void crawler_CrawlFinished(object sender, NCrawler.Events.CrawlFinishedEventArgs e)
        {
            log.Info("Crawling complete");
            if (this.config.Optimize)
            {
                log.Info("Optimizing index");
                this.repository.Optimize();
            } else
            {
                log.Info("Index optimization disabled");
            }

            repository.Dispose();
        }

        public void Process(Crawler crawler, PropertyBag propertyBag)
        {
            if (!bindevents)
            {
                crawler.CrawlFinished += new EventHandler<CrawlFinishedEventArgs>(crawler_CrawlFinished);
                bindevents = true;
            }

            string id = config.GetDocumentPath(propertyBag.Step.Uri);

            if (propertyBag.StatusCode == System.Net.HttpStatusCode.OK)
            {
                repository.AddUpdate(id, propertyBag.Title, propertyBag.Text, propertyBag.LastModified);
                log.Info("Add/Update [" + id + "]");

            } else if (propertyBag.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                log.Warning("Crawler encoutered 404 for [" + id + "]");
                repository.Delete(id);
            } else
            {
                log.Warning("Crawler encountered status " + propertyBag.StatusCode.ToString() + " (" + propertyBag.StatusDescription + ") for document " + id);
            }
        }
    }
}
