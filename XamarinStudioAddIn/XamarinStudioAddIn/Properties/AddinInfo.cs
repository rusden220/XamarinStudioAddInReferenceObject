using System;
using Mono.Addins;
using Mono.Addins.Description;

[assembly:Addin (
	"XamarinStudioAddIn", 
	Namespace = "XamarinStudioAddIn",
	Version = "1.0"
)]

[assembly:AddinName ("XamarinStudioAddIn")]
[assembly:AddinCategory ("IDE extensions")]
[assembly:AddinDescription ("XamarinStudioAddIn")]
[assembly:AddinAuthor ("DenS")]

//[assembly:AddinDependency ("Core", MonoDevelop.BuildInfo.Version)]
//[assembly:AddinDependency ("Ide", MonoDevelop.BuildInfo.Version)]
//[assembly:AddinDependency("Dubugger", MonoDevelop.BuildInfo.Version)]

