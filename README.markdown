# Total Recall #

> Crawl and index your (static) asp.net website using an MSBuild target. Simple querying. Uses Lucene.Net and NCrawler.

totalrecall is a kind of wrapper for Lucene, but with simplified indexing that takes place during the build process. You give the build target the address of your integration or development server, and documents are crawled and indexed using HTTP. The resulting Lucene index is stored in your site (on the file system) &mdash; a simple query wrapper knows where to look for it.

### Nuget Install ###

Step 1: Install package

    nuget> install-package totalrecall

Step 2: Edit packages\totalrecall\tools\totalrecall.targets file:

    <TotalRecall.MSBuild.IndexSiteTask PublishedWebsiteUrl="[YOUR NEWLY BUILT WEBSITE URL]" Optimize="True" IndexFolder="$(WebProjectOutputDir)\.totalrecall"/>

Step 3: Index your site:

    MSBuild.exe /target:CrawlIndexSite mywebsiteproj.csproj

Step 4: Navigate to /search/query=something%20interesting


### Manual Install ###

Step 1: Import totalrecall targets somewhere into your project file:

    <Import Project="..\lib\totalrecall.targets" />

Step 2: Edit packages\totalrecall\tools\totalrecall.targets file:

    <TotalRecall.MSBuild.IndexSiteTask PublishedWebsiteUrl="[YOUR NEWLY BUILT WEBSITE URL]" Optimize="True" IndexFolder="$(WebProjectOutputDir)\.totalrecall"/>

Step 3: Add config section to your web.config file:

    <configSections>
       <section name="totalrecall" type="TotalRecall.Configuration.TotalRecallConfigurationSection,TotalRecall" />
    </configSections>

    <totalrecall indexfolder="~/.totalrecall" />

Step 4: Index your site:

    MSBuild.exe /target:CrawlIndexSite mywebsiteproj.csproj

Step 5: Query on your search page:

    var engine = new TotalRecall.SearchEngine();
    var hits = engine.Search("something interesting", 10);

### web.config section documentation ###

*All values are optional*

    <totalrecall
      indexfolder="~/.totalrecall"  <!-- The path where the Lucene index should be stored. Default: "~/.totalrecall" -->
      siterootdirectory="store"     <!-- The virtual directory of your development site if there is one. Default: Empty -->
      optimize="true"               <!-- Whether or not to optimize and compact the index after crawling: Default: true -->
    />