using ImageResizer.Plugins.UniqueUrlFolderPresets.Configuration;

using NSubstitute;

using Shouldly;

using Xunit;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Tests
{
    public class PresetParserTests
    {
        [Theory]
        [InlineData("optimized/test/xxx/yyy/zzz.png", "test")]
        [InlineData("/optimized/test/xxx/yyy/zzz.png", "test")]
        [InlineData("/optimized/preset-with-dashes/xxx/yyy/zzz.png", "preset-with-dashes")]
        [InlineData("/optimized/preset-with-dashes_and_underscores/xxx/yyy/zzz.png", "preset-with-dashes_and_underscores")]
        [InlineData("/xxx/yyy/zzz.png", null)]
        [InlineData("xxx/yyy/zzz.png", null)]
        public void GetPresetFromPath(string url, string expectedPreset)
        {
            var configuration = Substitute.For<IUniqueUrlFolderPresetsConfiguration>();
            configuration.BaseSegment.Returns("optimized");
            var sut = new PresetParser(configuration);
            var preset = sut.GetPresetFromPath(url);
            preset.ShouldBe(expectedPreset);
        }
    }
}