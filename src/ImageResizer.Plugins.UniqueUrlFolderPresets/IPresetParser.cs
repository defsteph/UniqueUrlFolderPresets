namespace ImageResizer.Plugins.UniqueUrlFolderPresets
{
    public interface IPresetParser
    {
        string GetPresetFromPath(string path);
    }
}