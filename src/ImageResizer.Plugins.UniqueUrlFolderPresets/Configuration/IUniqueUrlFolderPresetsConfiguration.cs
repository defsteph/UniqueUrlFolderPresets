using System;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Configuration
{
    public interface IUniqueUrlFolderPresetsConfiguration
    {
        string BaseSegment { get; }
        TimeSpan CacheMaxAge { get; }
        bool Enabled { get; }
    }
}