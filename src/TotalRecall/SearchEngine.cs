using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Index;
using System.Web.Mvc;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis;
using Lucene.Net.Search;
using TotalRecall.Configuration;
using System.Configuration;
using Lucene.Net.Store;
using System.IO;

namespace TotalRecall
{
    public class SearchEngine
    {
        private readonly IndexSearcher searcher;

        public IEnumerable<Hit> Search(string query, int maxResults)
        {
            QueryParser qp = new QueryParser(Lucene.Net.Util.Version.LUCENE_29, "contents", new SimpleAnalyzer());
            Query q = qp.Parse(query);

            TopDocs top = searcher.Search(q, maxResults);
            List<Hit> result = new List<Hit>(top.totalHits);

            for (int index = 0; index < top.totalHits; index++)
            {
                var doc = searcher.Doc(top.scoreDocs[index].doc);
                result.Add(new Hit()
                {
                    Relevance = top.scoreDocs[index].score,
                    Title = doc.GetField("title").StringValue(),
                    Url = doc.GetField("path").StringValue()
                });
            }

            return result;
        }

        public SearchEngine()
        {
            var config = (TotalRecallConfigurationSection)ConfigurationManager.GetSection("totalRecall");
            searcher = new IndexSearcher(FSDirectory.Open(new DirectoryInfo(config.IndexFolder)), true);
        }
    }
}
