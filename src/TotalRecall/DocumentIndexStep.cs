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

namespace TotalRecall
{
    class DocumentIndexStep : IPipelineStep
    {
        private log4net.ILog log;
        private IConfig config;
        private DocumentRepository repository;
        private bool bindevents = false;

        public DocumentIndexStep(IConfig config, log4net.ILog log)
        {
            this.config = config;
            this.log = log;

            this.repository = new DocumentRepository(
                new IndexWriter(
                    FSDirectory.Open(new DirectoryInfo(config.IndexFolder)),
                    new SimpleAnalyzer(),
                    true,
                    IndexWriter.MaxFieldLength.UNLIMITED
                )
            );
        }

        private void crawler_CrawlFinished(object sender, NCrawler.Events.CrawlFinishedEventArgs e)
        {
            if (this.config.Optimize)
            {
                log.Debug("Optimizing index");
                this.repository.Optimize();
            }
#if DEBUG
            log.Debug("Disposing repository");
#endif
            repository.Dispose();
        }

        public void Process(Crawler crawler, PropertyBag propertyBag)
        {
            if (!bindevents)
            {
                crawler.CrawlFinished += new EventHandler<NCrawler.Events.CrawlFinishedEventArgs>(crawler_CrawlFinished);
                bindevents = true;
            }

            string id = config.GetDocumentPath(propertyBag.Step.Uri);

            if (propertyBag.StatusCode == System.Net.HttpStatusCode.OK)
            {
                repository.AddUpdate(id, propertyBag.Title, propertyBag.Text, propertyBag.LastModified);
#if DEBUG
                log.Debug("Add/Update doc id [" + id + "]");
#endif

            } else if (propertyBag.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                log.Debug("Crawler encoutered 404: " + id + ", deleting document");
                repository.Delete(id);
            } else
            {
                log.Debug("Crawler encountered status " + propertyBag.StatusCode.ToString() + " (" + propertyBag.StatusDescription + "): " + id);
            }
        }
    }
}
