using System.Text.RegularExpressions;
using ImageResizer.Plugins.UniqueUrlFolderPresets.Configuration;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets
{
    internal class PresetParser : IPresetParser
    {
        private const string PresetFolderExpressionFormat = "(?<=^|\\/){0}\\/(?<preset>\\w+)\\/";
        private readonly Regex _presetFolderExpression;
        public PresetParser(IUniqueUrlFolderPresetsConfiguration configuration)
        {
            _presetFolderExpression = new Regex(string.Format(PresetFolderExpressionFormat, configuration.BaseSegment), RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        }
        public string GetPresetFromPath(string path)
        {
            var match = _presetFolderExpression.Match(path);
            if (match.Success)
            {
                return match.Groups["preset"].Value.ToLowerInvariant();
            }
            return null;
        }
    }
}