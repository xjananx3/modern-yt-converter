using Windows.Storage.Pickers;

namespace YouTubeDownloader;

public class FileProvider
{
    public async Task<string> ChooseDownloadFolderAsync()
    {
        var folderPicker = new FolderPicker
        {
            SuggestedStartLocation = PickerLocationId.Desktop
        };
        folderPicker.FileTypeFilter.Add("*");
        var folder = await folderPicker.PickSingleFolderAsync();
        
        return folder.Path;
    }
}
