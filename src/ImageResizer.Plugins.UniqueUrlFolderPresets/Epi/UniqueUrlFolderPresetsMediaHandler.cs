using System;
using System.Web;

using EPiServer.Core;
using EPiServer.Framework.Blobs;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;

using ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Epi
{
    [TemplateDescriptor(Inherited = true, TemplateTypeCategory = TemplateTypeCategories.HttpHandler, Default = true)]
    public class UniqueUrlFolderPresetsMediaHandler : BlobHttpHandler, IRenderTemplate<IContentMedia>
    {
        protected override Blob GetBlob(HttpContextBase httpContext)
        {
            AppendContentDispositionHeaderIfDownloadRequest(httpContext);
            var binaryStorable = ServiceLocator.Current.GetInstance<IContentRouteHelper>().Content as IBinaryStorable;
            return binaryStorable != null ? binaryStorable.BinaryData : null;
        }
        private static void AppendContentDispositionHeaderIfDownloadRequest(HttpContextBase context)
        {
            var filename = context.Request.RequestContext.GetCustomRouteData<string>(RoutingConstants.DownloadSegment);
            if (!string.IsNullOrWhiteSpace(filename))
            {
                context.Response.AppendHeader("Content-Disposition", $"attachment; filename=\"{filename}\"; filename*=UTF-8''{Uri.EscapeDataString(filename)}");
            }
        }
        protected override void SetCachePolicy(HttpContextBase context, DateTime fileChangedDate)
        {
            if (!IsAnonymousRequestForMediaWithVersionHash(context))
            {
                base.SetCachePolicy(context, fileChangedDate);
                return;
            }
            var redirects = ServiceLocator.Current.GetInstance<IContentHashHelper>().RedirectIfWrongHash(context, ServiceLocator.Current.GetInstance<IContentRouteHelper>().Content);
            if (!redirects)
            {
                ServiceLocator.Current.GetInstance<ICacheHelper>().SetCacheHeaders(context, fileChangedDate, "EPiServer");
            }
        }
        private static bool IsAnonymousRequestForMediaWithVersionHash(HttpContextBase context)
        {
            return !context.Request.IsAuthenticated && context.Items[UniqueUrlContext.VersionHashRequestKey] != null;
        }
    }
}