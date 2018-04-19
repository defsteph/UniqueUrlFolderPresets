using System;
using System.Text.RegularExpressions;
using System.Web;

using EPiServer.Core;

using ImageResizer.Plugins.UniqueUrlFolderPresets.Configuration;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers
{
    internal class ContentHashHelper : IContentHashHelper
    {
        private const string HashPattern = "^/[a-z0-9]{8}";
        private static readonly Regex HashedUrlExpression = new Regex(HashPattern + "/.+", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
        private static readonly Regex ContentHashExpression = new Regex(HashPattern, RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);

        private readonly IUniqueUrlFolderPresetsConfiguration _configuration;
        public ContentHashHelper(IUniqueUrlFolderPresetsConfiguration configuration)
        {
            _configuration = configuration;
        }
        public int HashLength => 8;
        public string GetSaveDateHash(IContent content)
        {
            var saveDate = ((IChangeTrackable)content).Saved;
            return saveDate.GetHashCode().ToString("x8");
        }
        public bool IsHashedPath(string path)
        {
            return HashedUrlExpression.IsMatch(path);
        }
        public bool RedirectIfWrongHash(HttpContextBase context, IContent content)
        {
            if (content != null)
            {
                var requestedHash = (string)context.Items[UniqueUrlContext.VersionHashRequestKey];
                var currentHash = GetSaveDateHash(content);
                if (!string.Equals(requestedHash, currentHash, StringComparison.OrdinalIgnoreCase))
                {
                    var request = context.Request;
                    var preset = request.QueryString["preset"];
                    if (!string.IsNullOrWhiteSpace(preset))
                    {
                        context.Response.RedirectPermanent($"/{_configuration.BaseSegment}/{preset}/{currentHash}{request.Path}");
                        return true;
                    }
                    context.Response.RedirectPermanent($"/{currentHash}{request.Url?.PathAndQuery}");
                    return true;
                }
            }
            return false;
        }
        public bool RedirectIfWrongHash(HttpContext context, IContent content)
        {
            return RedirectIfWrongHash(new HttpContextWrapper(context), content);
        }

        public string RemoveHash(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                return path;
            }
            return ContentHashExpression.Replace(path, string.Empty);
        }
    }
}