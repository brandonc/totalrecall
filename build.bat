"%SystemRoot%\Microsoft.NET\Framework\v4.0.30319\msbuild.exe" "%~dp0\totalrecall.proj" /t:Clean;Build;CopyToBinFolder;Merge;MergeRekallConsole;CopyToNugetPackage
