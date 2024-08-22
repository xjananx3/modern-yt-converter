using Windows.Storage.Pickers;

namespace YouTubeDownloader.Assets;

public class FileProvider
{
    public async Task<string> ChooseDownloadFolderAsync()
    {
        var folderPicker = new FolderPicker();
        folderPicker.FileTypeFilter.Add("*");
        var folder = await folderPicker.PickSingleFolderAsync();
        
        return folder.Path;
    }
}
