using ImageResizer.Plugins.UniqueUrlFolderPresets.Configuration;
using ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers;
using NSubstitute;
using Shouldly;
using Xunit;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Tests
{
    public class ContentHashHelperTests
    {
        [Theory]
        [InlineData(null, null)]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData("xxx/yyy/zzz.png", "xxx/yyy/zzz.png")]
        [InlineData("/xxx/yyy/zzz.png", "/xxx/yyy/zzz.png")]
        [InlineData("/65f134e5/xxx/yyy/zzz.png", "/xxx/yyy/zzz.png")]
        public void RemoveHash(string url, string expected)
        {
            var configuration = Substitute.For<IUniqueUrlFolderPresetsConfiguration>();
            configuration.BaseSegment.Returns("optimized");
            var sut = new ContentHashHelper(configuration);
            var actual = sut.RemoveHash(url);
            actual.ShouldBe(expected);
        }
    }
}