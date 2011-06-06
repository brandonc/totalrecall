using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TotalRecall.Configuration;

namespace TotalRecall
{
    static class IConfigExtensions
    {
        public static string GetDocumentPath(this IConfig config, Uri absoluteUrl)
        {
            string pq = absoluteUrl.PathAndQuery;
            string result = pq;
            if (!String.IsNullOrEmpty(config.SiteRootDirectory) && pq.StartsWith(config.SiteRootDirectory))
            {
                result = pq.Substring(config.SiteRootDirectory.Length);
                while (result.StartsWith("/"))
                {
                    result = result.Substring(1);
                }
            }

            return result;
        }
    }
}
