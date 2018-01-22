using System;
using System.Collections.Generic;
using System.Web;

using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;

using ImageResizer.Configuration;
using ImageResizer.Configuration.Xml;
using ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets
{
    public class UniqueUrlFolderPresetsPlugin : IPlugin
    {
        private readonly List<string> _presetNames = new List<string>();
        public IPlugin Install(Config config)
        {
            config.Plugins.add_plugin(this);
            ReadPresetNamesFromConfig(config.getConfigXml().queryFirst("presets"));
            config.Pipeline.RewriteDefaults += OnRewriteDefaults;
            config.Pipeline.PreHandleImage += OnPreHandleImage;
            return this;
        }
        private void ReadPresetNamesFromConfig(Node presetConfigNode) {
            if(presetConfigNode?.Children == null) {
                return;
            }
            foreach(var presetNode in presetConfigNode.Children) {
                if(presetNode.Name.Equals("preset", StringComparison.OrdinalIgnoreCase)) {
                    var name = presetNode.Attrs["name"];
                    _presetNames.Add(name.ToLowerInvariant());
                }
            }
        }
        private void OnRewriteDefaults(IHttpModule sender, HttpContext context, IUrlEventArgs e)
        {
            var preset = e.QueryString["preset"];
            if (!string.IsNullOrWhiteSpace(preset) && !_presetNames.Contains(preset.ToLowerInvariant()))
            {
                throw new HttpException(404, "Not Found");
            }
            ServiceLocator.Current.GetInstance<IContentHashHelper>().RedirectIfWrongHash(new HttpContextWrapper(context), ServiceLocator.Current.GetInstance<IContentRouteHelper>().Content);
        }
        private static void OnPreHandleImage(IHttpModule sender, HttpContext context, Caching.IResponseArgs e)
        {
            ServiceLocator.Current.GetInstance<ICacheHelper>().SetCacheHeaders(context, GetLastModifiedFromRequestKey(e.RequestKey), "ImageResizer" );
        }
        private static DateTime? GetLastModifiedFromRequestKey(string requestKey)
        {
            var dateIndex = requestKey.IndexOf("|", StringComparison.Ordinal); 
            if (dateIndex > -1)
            {
                long ticks;
                if (long.TryParse(requestKey.Substring(dateIndex + 1), out ticks))
                {
                    return new DateTime(ticks);
                }
            }
            return null;
        }
        public bool Uninstall(Config config)
        {
            config.Plugins.remove_plugin(this);
            config.Pipeline.RewriteDefaults -= OnRewriteDefaults;
            config.Pipeline.PreHandleImage -= OnPreHandleImage;
            return true;
        }
    }
}