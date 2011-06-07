# Total Recall #

> Lucene-powered site crawler, indexer, and query interface. With MSBuild integration.

Step 1: Edit output\totalrecall.targets file:

    <TotalRecall.IndexSiteTask PublishedWebsiteUrl="[Your website url]" Optimize="True" IndexFolder="$(WebProjectOutputDir)\.totalrecall"/>

Step 2: Import totalrecall targets somewhere into your project file:

    <Import Project="..\totalrecall\output\totalrecall.targets" />

Step 3: Add config section to your web.config file:

    <configSections>
       <section name="totalrecall" type="TotalRecall.Configuration.TotalRecallConfigurationSection,TotalRecall" />
    </configSections>

    <totalrecall IndexFolder=".totalrecall" />

Step 4: Index your site:

    MSBuild.exe /target:CrawlIndexSite mywebsiteproj.csproj

Step 5: Query on your search page:

    var engine = new TotalRecall.SearchEngine();
    var hits = engine.Search("Some query", 10);
