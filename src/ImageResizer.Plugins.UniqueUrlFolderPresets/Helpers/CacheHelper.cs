using System;
using System.Web;

using ImageResizer.Plugins.UniqueUrlFolderPresets.Configuration;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers
{
    internal class CacheHelper : ICacheHelper
    {
        private readonly IUniqueUrlFolderPresetsConfiguration _configuration;
        public CacheHelper(IUniqueUrlFolderPresetsConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void SetCacheHeaders(HttpContextBase context, DateTime? fileChanged, string handlerName)
        {
            context.Response.Cache.SetCacheability(context.Request.IsAuthenticated ? HttpCacheability.Private : HttpCacheability.Public);
            if (fileChanged.HasValue)
            {
                context.Response.Cache.SetLastModified(fileChanged.Value);
            }
            context.Response.Cache.SetMaxAge(_configuration.CacheMaxAge);
            context.Response.Cache.VaryByParams.IgnoreParams = true;
            context.Response.Cache.SetOmitVaryStar(true);
            context.Response.Cache.SetNoServerCaching();
            if (context.IsDebuggingEnabled)
            {
                context.Response.AppendHeader("X-MediaHandler", handlerName);
            }
        }
        public void SetCacheHeaders(HttpContext context, DateTime? fileChanged, string handlerName)
        {
            SetCacheHeaders(new HttpContextWrapper(context), fileChanged, handlerName);
        }
    }
}