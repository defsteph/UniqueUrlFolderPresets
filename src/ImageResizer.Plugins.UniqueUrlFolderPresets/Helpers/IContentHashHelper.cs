using System.Web;

using EPiServer.Core;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers
{
    public interface IContentHashHelper
    {
        int HashLength { get; }
        string GetSaveDateHash(IContent content);
        bool IsHashedPath(string path);
        bool RedirectIfWrongHash(HttpContextBase context, IContent content);
        bool RedirectIfWrongHash(HttpContext context, IContent content);
    }
}