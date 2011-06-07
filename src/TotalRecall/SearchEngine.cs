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
using Lucene.Net.Analysis.Standard;
using Lucene.Net.Search.Spans;
using Lucene.Net.Highlight;

namespace TotalRecall
{
    public class SearchEngine
    {
        private readonly IndexSearcher searcher;

        public IEnumerable<Hit> Search(string query, int maxResults)
        {
            var analyzer = new StandardAnalyzer(Lucene.Net.Util.Version.LUCENE_29);

            QueryParser qp = new QueryParser(
                Lucene.Net.Util.Version.LUCENE_29,
                "contents",
                analyzer
            );
            Query q = qp.Parse(query);

            TopDocs top = searcher.Search(q, maxResults);
            List<Hit> result = new List<Hit>(top.totalHits);

            for (int index = 0; index < top.totalHits; index++)
            {
                var doc = searcher.Doc(top.scoreDocs[index].doc);
                string contents = doc.Get("contents");

                var scorer = new QueryScorer(q, searcher.GetIndexReader(), "contents");
                var highlighter = new Highlighter(scorer);

                result.Add(new Hit()
                {
                    Relevance = top.scoreDocs[index].score,
                    Title = doc.Get("title"),
                    Url = doc.Get("path"),
                    Excerpt = highlighter.GetBestFragment(analyzer, "contents", contents)
                });
            }

            return result;
        }

        public SearchEngine()
        {
            var config = (TotalRecallConfigurationSection)ConfigurationManager.GetSection("totalrecall");

            if (config == null)
            {
                config = new TotalRecallConfigurationSection()
                {
                    IndexFolder = TotalRecallConfigurationSection.DefaultIndexFolderName,
                    Optimize = TotalRecallConfigurationSection.DefaultOptimize
                };
            }

            searcher = new IndexSearcher(FSDirectory.Open(new DirectoryInfo(config.IndexFolder)), true);
        }
    }
}
