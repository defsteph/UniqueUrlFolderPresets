using System;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Configuration
{
    internal class UniqueUrlFolderPresetsAppSettingsConfiguration : IUniqueUrlFolderPresetsConfiguration
    {
        public string BaseSegment => System.Configuration.ConfigurationManager.AppSettings["uufp:BaseSegment"] ?? "optimized";
        public TimeSpan CacheMaxAge => TimeSpan.Parse(System.Configuration.ConfigurationManager.AppSettings["uufp:CacheMaxAgeTimeSpan"] ?? "365.0:0:0");
    }
}