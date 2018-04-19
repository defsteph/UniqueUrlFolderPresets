using System.Web.Mvc;

using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc.Html;

using ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Mvc
{
    public static class UrlHelperExtensions
    {
        public static string ResizeImage(this UrlHelper urlHelper, ContentReference contentLink, string preset = null)
        {
            if (ContentReference.IsNullOrEmpty(contentLink))
            {
                return null;
            }
            var url = urlHelper.ContentUrl(contentLink);
            if (string.IsNullOrWhiteSpace(preset))
            {
                return url;
            }

            return urlHelper.PrependResizingInstruction(url, preset);
        }

        public static string PrependResizingInstruction(this UrlHelper urlHelper, string url, string preset)
        {
            return ServiceLocator.Current.GetInstance<IResizeHelper>().PrependResizingInstruction(url, preset);
        }
    }
}