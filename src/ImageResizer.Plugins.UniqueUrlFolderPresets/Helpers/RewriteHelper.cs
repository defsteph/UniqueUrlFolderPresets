using System;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

using EPiServer.Core;
using EPiServer.Web.Routing;

using ImageResizer.Plugins.UniqueUrlFolderPresets.Configuration;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers
{
    internal class RewriteHelper : IRewriteHelper
    {
        private static readonly string[] SupportedMediaPaths = { "contentassets", RouteCollectionExtensions.SiteAssetStaticSegment, RouteCollectionExtensions.GlobalAssetStaticSegment };
        private readonly IUniqueUrlFolderPresetsConfiguration _configuration;
        private readonly IContentHashHelper _contentHashHelper;
        private readonly IPresetParser _presetParser;
        public RewriteHelper(IUniqueUrlFolderPresetsConfiguration configuration, IContentHashHelper contentHashHelper, IPresetParser presetParser)
        {
            _configuration = configuration;
            _contentHashHelper = contentHashHelper;
            _presetParser = presetParser;
        }
        public string[] MediaPaths => SupportedMediaPaths;
        public void RewriteContentHashOrPresetPath(HttpContext context)
        {
            RewriteContentHashOrPresetPath(new HttpContextWrapper(context));
        }
        public void RewriteContentHashOrPresetPath(HttpContextBase context)
        {
            var requestPath = context.Request.Path;
            var rewritten = RewriteHashPath(context, requestPath);
            if (!rewritten)
            {
                RewritePresetPath(context, requestPath);
            }
        }
        private bool RewriteHashPath(HttpContextBase context, string requestPath)
        {
            if (_contentHashHelper.IsHashedPath(requestPath))
            {
                var pathWithoutHash = requestPath.Substring(_contentHashHelper.HashLength + 2);
                if (SupportedMediaPaths.Any(path => pathWithoutHash.StartsWith(path)))
                {
                    context.Items[UniqueUrlContext.VersionHashRequestKey] = requestPath.Substring(1, _contentHashHelper.HashLength);
                    context.RewritePath("/" + pathWithoutHash, string.Empty, context.Request.QueryString.ToString(), true);
                    return true;
                }
            }
            return false;
        }
        private void RewritePresetPath(HttpContextBase context, string requestPath)
        {
            var preset = _presetParser.GetPresetFromPath(requestPath);
            if (!string.IsNullOrWhiteSpace(preset))
            {
                var presetResizingParametersLength = _configuration.BaseSegment.Length + 3 + preset.Length;
                var pathWithoutResizingParameters = requestPath.Substring(presetResizingParametersLength - 1);
                string hash;
                string actualPath;
                var hasHash = _contentHashHelper.IsHashedPath(pathWithoutResizingParameters);
                if (!hasHash)
                {
                    hash = new string('a', _contentHashHelper.HashLength);
                    actualPath = pathWithoutResizingParameters.Substring(1);
                }
                else
                {
                    hash = requestPath.Substring(presetResizingParametersLength, _contentHashHelper.HashLength);
                    actualPath = requestPath.Substring(presetResizingParametersLength + _contentHashHelper.HashLength + 1);
                }
                if (SupportedMediaPaths.Any(path => actualPath.StartsWith(path)))
                {
                    context.Items[UniqueUrlContext.VersionHashRequestKey] = hash;
                    var queryString = new NameValueCollection(context.Request.QueryString) {{"preset", preset}};
                    context.RewritePath("/" + actualPath, string.Empty, GetQueryString(queryString), true);
                }
            }
        }
        private static string GetQueryString(NameValueCollection nvc)
        {
            return string.Join("&", nvc.AllKeys.Select(key => key + "=" + HttpUtility.UrlEncode(nvc[key])));
        }
        public Uri GetHashedUri(IContent content, string path)
        {
            return new Uri($"/{_contentHashHelper.GetSaveDateHash(content)}/{path}", UriKind.RelativeOrAbsolute);
        }
    }
}