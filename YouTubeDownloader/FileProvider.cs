using System.Text;
using System.Xml;
using Windows.Storage.Pickers;

namespace YouTubeDownloader;

public class FileProvider
{
    public async Task<string> ChooseDownloadFolderAsync()
    {
        try
        {
            var folderPicker = new FolderPicker
            {
                SuggestedStartLocation = PickerLocationId.Desktop
            };
            folderPicker.FileTypeFilter.Add("*");
            var folder = await folderPicker.PickSingleFolderAsync();

            return folder?.Path;
        }
        catch (Exception ex)
        {
            string errorMessage = ex.Message;
            if (errorMessage.Contains("folder"))
            {
                string folderPrefix = "One or more errors occurred. (The folder ";
                string folderSuffix = " does not exist";

                int startIndex = errorMessage.IndexOf(folderPrefix) + folderPrefix.Length;
                int endIndex = errorMessage.IndexOf(folderSuffix);

                if (startIndex >= 0 && endIndex > startIndex)
                {
                    string encodedPath = errorMessage.Substring(startIndex, endIndex - startIndex);

                    return encodedPath;
                }
            }
            throw new Exception($"An unexpected error occurred: {ex.Message}");
        }
    }
}
