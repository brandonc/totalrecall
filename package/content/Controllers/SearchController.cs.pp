using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TotalRecall;

namespace $rootnamespace$.Controllers
{
    public class SearchController : Controller
    {
        //
        // GET: /Search/?query=something
        public ActionResult Index(string query, int totalResults = 10)
        {
            var engine = new SearchEngine();
            var hits = engine.Search(query, totalResults);

            return View(hits);
        }

    }
}
