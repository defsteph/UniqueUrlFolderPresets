using ImageResizer.Plugins.UniqueUrlFolderPresets.Configuration;
using ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Tests
{
    public class ResizeHelperTests
    {
        [Theory]
        [InlineData(null, "this", null)]
        [InlineData("", "this", "")]
        [InlineData(" ", "this", " ")]
        [InlineData("xxx/yyy/zzz.png", null, "xxx/yyy/zzz.png")]
        [InlineData("/xxx/yyy/zzz.png", null, "/xxx/yyy/zzz.png")]
        [InlineData("xxx/yyy/zzz.png", "this", "/optimized/this/xxx/yyy/zzz.png")]
        [InlineData("/xxx/yyy/zzz.png", "this", "/optimized/this/xxx/yyy/zzz.png")]
        [InlineData("/optimized/this/xxx/yyy/zzz.png", "this", "/optimized/this/xxx/yyy/zzz.png")]
        [InlineData("/optimized/that/xxx/yyy/zzz.png", "this", "/optimized/this/xxx/yyy/zzz.png")]
        public void PrependResizingInstruction(string url, string preset, string expected)
        {
            var configuration = Substitute.For<IUniqueUrlFolderPresetsConfiguration>();
            configuration.BaseSegment.Returns("optimized");
            var sut = new ResizeHelper(configuration, new PresetParser(configuration));
            var actual = sut.PrependResizingInstruction(url, preset);
            actual.ShouldBe(expected);
        }
    }
}