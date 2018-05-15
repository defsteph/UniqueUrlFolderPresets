using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

using EPiServer.PlugIn;

[assembly: AssemblyTitle("Unique Url Folder Presets")]
[assembly: AssemblyDescription("Unique, cachable, URLs for media in EPiServer using ImageResizing.NET presets.")]
[assembly: AssemblyCompany("Stephan Lonntorp")]
[assembly: AssemblyCopyright("Copyright 2018")]
[assembly: AssemblyProduct("ImageResizer.Plugins.UniqueUrlFolderPresets")]
[assembly: ComVisible(false)]
[assembly: Guid("9e00fd51-7775-4bb6-bf14-a8171c39b4fc")]
[assembly: AssemblyVersion("1.2.1.0")]
[assembly: AssemblyFileVersion("1.2.1.0")]
[assembly: InternalsVisibleTo("ImageResizer.Plugins.UniqueUrlFolderPresets.Tests")]
[assembly: PlugInSummary(MoreInfoUrl = "<A href='https://github.com/defsteph/UniqueUrlFolderPresets'>GitHub Repository</A>", License = LicensingMode.Freeware)]
#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif