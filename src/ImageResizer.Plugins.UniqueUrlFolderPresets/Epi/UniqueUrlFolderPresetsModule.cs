using System;
using System.Linq;
using System.Web;
using System.Web.Routing;

using EPiServer;
using EPiServer.Core;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Security;
using EPiServer.Web;
using EPiServer.Web.Routing;

using ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Epi
{
    [ModuleDependency(typeof(UniqueUrlFolderPresetsInitialization))]
    public class UniqueUrlFolderPresetsModule : IInitializableHttpModule
    {
        private IContentLoader _contentLoader;
        private IRewriteHelper _rewriteHelper;
        public void Initialize(InitializationEngine context)
        {
            _rewriteHelper = context.Locate.Advanced.GetInstance<IRewriteHelper>();
            _contentLoader = context.Locate.Advanced.GetInstance<IContentLoader>();
            context.Locate.Advanced.GetInstance<IContentRouteEvents>().CreatedVirtualPath += OnCreatedVirtualPath;
        }
        private void OnCreatedVirtualPath(object sender, UrlBuilderEventArgs e)
        {
            if (IsRequestForMediaInDefaultContext(e))
            {
                var content = GetReadableContentFromRoute(e.RouteValues);
                if (content != null)
                {
                    e.UrlBuilder.Uri = _rewriteHelper.GetHashedUri(content, e.UrlBuilder.Path);
                }
            }
        }
        private bool IsRequestForMediaInDefaultContext(UrlBuilderEventArgs urlBuilderEventArgs)
        {
            return urlBuilderEventArgs.RequestContext.GetContextMode() == ContextMode.Default && _rewriteHelper.MediaPaths.Any(path => urlBuilderEventArgs.UrlBuilder.Path.StartsWith(path));
        }
        private IContent GetReadableContentFromRoute(RouteValueDictionary routeValues)
        {
            var contentLink = routeValues[RoutingConstants.NodeKey] as ContentReference;
            IContent content;
            if (_contentLoader.TryGet(contentLink, out content) && ((ISecurable)content).GetSecurityDescriptor().HasAccess(PrincipalInfo.AnonymousPrincipal, AccessLevel.Read))
            {
                return content;
            }
            return null;
        }
        public void Uninitialize(InitializationEngine context)
        {
            _rewriteHelper = null;
            _contentLoader = null;
            context.Locate.Advanced.GetInstance<IContentRouteEvents>().CreatedVirtualPath -= OnCreatedVirtualPath;
        }
        public void InitializeHttpEvents(HttpApplication application)
        {
            application.BeginRequest += OnBeginRequest;
        }
        private void OnBeginRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            _rewriteHelper.RewriteContentHashOrPresetPath(context);
        }
    }
}