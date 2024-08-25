using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using Windows.Services.Maps.OfflineMaps;
using YouTubeDownloader;

namespace YouTubeDownloader;

public sealed partial class MainPage : Page
{
    private readonly FileProvider _fileProvider = new();
    private readonly DownloadManager _downloadManager;
    public MainPage()
    {
        this.InitializeComponent();
        _downloadManager = new DownloadManager();
    }

    private async void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        var folderPath = await _fileProvider.ChooseDownloadFolderAsync();
        FolderPathTextBox.Text = folderPath;
    }

    private async void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
        var arguments = GetArguments();
        
        try
        {
            await _downloadManager.StartDownloadAsync(arguments);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    private string GetArguments()
    {
        string folderArg = @"-o ";
        string filename = @"/%(title)s.%(ext)s ";
        string url = YouTubeLinkTextBox.Text;

        string fileTypeArg = AudioFormatRadioButton.IsChecked == true ? GetAudioArguments() : GetVideoArguments();


        string arguments = folderArg + FolderPathTextBox.Text + filename + fileTypeArg + url;
        return arguments;
    }

    private string GetAudioArguments()
    {
        string format = ((ComboBoxItem)AudioFormatComboBox.SelectedItem).Content.ToString()!.ToLower();
        
        return $"-x --audio-format {format} ";
    }

    private string GetVideoArguments()
    {
        string quality = GetQualityArgument();

        return $"-f {quality} ";
    }

    private string GetQualityArgument()
    {
        string quality = ((ComboBoxItem)QualityComboBox.SelectedItem).Content.ToString()!;
        
        return quality switch
        {
            "480p" => "bestvideo[height<=480]+bestaudio/best[height<=480]",
            "720p" => "bestvideo[height<=720]+bestaudio/best[height<=720]",
            "1080p" => "bestvideo[height<=1080]+bestaudio/best[height<=1080]",
            "Best" => "bestvideo[ext=mp4]+bestaudio[ext=m4a]/best[ext=mp4]/best",
            _ => ""
        };
    }
}
