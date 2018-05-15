using System;
using ImageResizer.Plugins.UniqueUrlFolderPresets.Configuration;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers
{
    internal class ResizeHelper : IResizeHelper
    {
        private readonly IUniqueUrlFolderPresetsConfiguration _configuration;
        private readonly IPresetParser _presetParser;

        public ResizeHelper(IUniqueUrlFolderPresetsConfiguration configuration, IPresetParser presetParser)
        {
            _configuration = configuration;
            _presetParser = presetParser;
        }

        public string PrependResizingInstruction(string url, string preset)
        {
            if (!_configuration.Enabled || string.IsNullOrWhiteSpace(url) || string.IsNullOrWhiteSpace(preset))
            {
                return url;
            }
            var existingPreset = _presetParser.GetPresetFromPath(url);
            if (existingPreset != null)
            {
                if (preset.Equals(existingPreset, StringComparison.OrdinalIgnoreCase))
                {
                    return url;
                }
                return url.Replace($"{_configuration.BaseSegment}/{existingPreset}", $"{_configuration.BaseSegment}/{preset}");
            }
            var resizingInstruction = $"/{_configuration.BaseSegment}/{preset}";
            if (url.StartsWith("/"))
            {
                return resizingInstruction + url;
            }
            return resizingInstruction + "/" + url;
        }
    }
}