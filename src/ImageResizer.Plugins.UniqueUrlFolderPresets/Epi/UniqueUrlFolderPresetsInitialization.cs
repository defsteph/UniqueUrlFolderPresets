using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;

using ImageResizer.Plugins.UniqueUrlFolderPresets.Configuration;
using ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers;

namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Epi
{
    [InitializableModule, ModuleDependency(typeof(FrameworkInitialization))]
    public class UniqueUrlFolderPresetsInitialization : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.ConfigurationComplete += (o, e) =>
            {
                context.Services
                    .AddSingleton<IUniqueUrlFolderPresetsConfiguration, UniqueUrlFolderPresetsAppSettingsConfiguration>()
                    .AddSingleton<IPresetParser, PresetParser>()
                    .AddSingleton<IContentHashHelper, ContentHashHelper>()
                    .AddSingleton<IRewriteHelper, RewriteHelper>()
                    .AddSingleton<ICacheHelper, CacheHelper>();
            };
        }
        public void Initialize(InitializationEngine context) { }
        public void Uninitialize(InitializationEngine context) { }
        public void Preload(string[] parameters) { }
    }
}