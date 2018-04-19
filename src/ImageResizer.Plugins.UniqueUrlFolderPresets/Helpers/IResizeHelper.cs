namespace ImageResizer.Plugins.UniqueUrlFolderPresets.Helpers
{
    public interface IResizeHelper
    {
        string PrependResizingInstruction(string url, string preset);
    }
}