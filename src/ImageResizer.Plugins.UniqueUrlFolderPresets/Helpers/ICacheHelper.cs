using System;
using System.Web;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers
{
    public interface ICacheHelper
    {
        void SetCacheHeaders(HttpContextBase context, DateTime? fileChanged, string handlerName);
        void SetCacheHeaders(HttpContext context, DateTime? fileChanged, string handlerName);
    }
}