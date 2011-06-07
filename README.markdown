# Total Recall #

> Lucene-powered site crawler, indexer, and query interface. With MSBuild integration.

Step 1: Edit output\totalrecall.targets file:

    &lt;TotalRecall.IndexSiteTask PublishedWebsiteUrl="[Your website url]" Optimize="True" IndexFolder="$(WebProjectOutputDir)\.totalrecall"/&gt;

Step 2: Import totalrecall targets somewhere into your project file:

    &lt;Import Project="..\totalrecall\output\totalrecall.targets" /&gt;

Step 3: Add config section to your web.config file:

    &lt;configSections&gt;
       &lt;section name="totalrecall" type="TotalRecall.Configuration.TotalRecallConfigurationSection,TotalRecall" /&gt;
    &lt;/configSections&gt;

    &lt;totalrecall IndexFolder=".totalrecall" /&gt;

Step 4: Index your site:

    MSBuild.exe /target:CrawlIndexSite mywebsiteproj.csproj

Step 5: Query on your search page:

    var engine = new TotalRecall.SearchEngine();
    var hits = engine.Search("Some query", 10);
