using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lucene.Net.Index;
using Lucene.Net.Documents;
using Lucene.Net.Search;
using Lucene.Net.QueryParsers;
using Lucene.Net.Analysis;

namespace TotalRecall
{
    class DocumentRepository : IDisposable
    {
        private IndexWriter index;
        private IndexReader reader;
        private IndexSearcher searcher;
        private bool finalized;

        private static readonly DateTime epoch = new DateTime(1970, 1, 1);

        public Document Find(string id)
        {
            TermQuery query = new TermQuery(new Term("path", id));
            TopDocs docs = searcher.Search(query, null, 1);

            if (docs.totalHits >= 1)
            {
                return searcher.Doc(docs.scoreDocs[0].doc);
            }
            return null;
        }

        public void Optimize()
        {
            index.Optimize();
        }

        public void Delete(string id)
        {
            index.DeleteDocuments(new Term("path", id));
        }

        public void AddUpdate(string id, string title, string contents, DateTime lastModified)
        {
            Document doc = Find(id);

            if (doc != null)
            {
                if ((long)(lastModified - epoch).TotalMilliseconds < DateTools.StringToTime(doc.GetField("modified").StringValue()))
                    return;

                index.DeleteDocuments(new Term("path", doc.GetField("path").StringValue()));
            }

            doc = new Document();
            doc.Add(new Field("path", id, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("title", title, Field.Store.YES, Field.Index.ANALYZED));
            doc.Add(new Field("contents", contents, Field.Store.NO, Field.Index.ANALYZED));
            doc.Add(new Field("modified", DateTools.TimeToString((long)(DateTime.Now - epoch).TotalMilliseconds, DateTools.Resolution.MINUTE), Field.Store.YES, Field.Index.NOT_ANALYZED));

            this.index.AddDocument(doc);
        }

        public DocumentRepository(IndexWriter index)
        {
            this.index = index;
            this.reader = this.index.GetReader();
            this.searcher = new IndexSearcher(this.reader);
        }

        ~DocumentRepository()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (!finalized)
            {
                index.Commit();
                reader.Close();
                searcher.Close();
                finalized = true;
                GC.SuppressFinalize(this);
            }
        }
    }
}
