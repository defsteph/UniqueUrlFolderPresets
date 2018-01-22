using System;
using System.Web;

using EPiServer.Core;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers
{
    public interface IRewriteHelper
    {
        string[] MediaPaths { get; }
        void RewriteContentHashOrPresetPath(HttpContextBase context);
        void RewriteContentHashOrPresetPath(HttpContext context);
        Uri GetHashedUri(IContent content, string path);
    }
}