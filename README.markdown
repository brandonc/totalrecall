# Total Recall #

> Lucene-powered site crawler, indexer, and query interface. With MSBuild integration.

1. Edit output\totalrecall.targets file:

    <TotalRecall.IndexSiteTask PublishedWebsiteUrl="[Your website url]" Optimize="True" IndexFolder="$(WebProjectOutputDir)\.totalrecall"/>

2. Import totalrecall targets somewhere into your project file:

    <Import Project="..\totalrecall\output\totalrecall.targets" />

3. Add config section to your web.config file:

    <configSections>
      <section name="totalrecall" type="TotalRecall.Configuration.TotalRecallConfigurationSection,TotalRecall" />
    </configSections>

    <totalrecall IndexFolder=".totalrecall" />

4. Index your site:

    MSBuild.exe /target:CrawlIndexSite mywebsiteproj.csproj

5. Query on your search page:

    var engine = new TotalRecall.SearchEngine();
    var hits = engine.Search("Some query", 10);
