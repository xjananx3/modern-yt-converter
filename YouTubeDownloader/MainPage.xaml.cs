using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using Windows.Services.Maps.OfflineMaps;
using YouTubeDownloader;

namespace YouTubeDownloader;

public sealed partial class MainPage : Page
{
    private readonly FileProvider _fileProvider = new();
    private readonly DownloadManager _downloadManager;
    private CancellationTokenSource _cancellationTokenSource;
    
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
        _cancellationTokenSource = new CancellationTokenSource();
        DownloadProgressBar.Visibility = Visibility.Visible;
        CancelButton.Visibility = Visibility.Visible;
        DownloadButton.Visibility = Visibility.Collapsed;
        StatusLabel.Text = "Downloading...";
        
        var arguments = GetArguments();

        try
        {
            await _downloadManager.StartDownloadAsync(arguments, _cancellationTokenSource.Token);
            StatusLabel.Text = "Download finished";
        }
        catch (OperationCanceledException)
        {
            StatusLabel.Text = "Download canceled";
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
            DownloadProgressBar.Visibility = Visibility.Collapsed;
            CancelButton.IsEnabled = false;
            DownloadButton.IsEnabled = true;
            DownloadButton.Visibility = Visibility.Visible;
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
        string extension = GetVideoExtension();
        string videoAudioExtension = GetVideoAudioExtension();
        
        return quality switch
        {
            "480p" => $"bestvideo{extension}[height<=480]+bestaudio{videoAudioExtension}/best{extension}[height<=480]",
            "720p" => $"bestvideo{extension}[height<=720]+bestaudio{videoAudioExtension}/best{extension}[height<=720]",
            "1080p" => $"bestvideo{extension}[height<=1080]+bestaudio{videoAudioExtension}/best{extension}[height<=1080]",
            "Best" => $"bestvideo{extension}+bestaudio{videoAudioExtension}/best{extension}/best",
            _ => ""
        };
    }

    private string GetVideoExtension()
    {
        bool isWebMFormat = (bool)WebMFormatCheckBox.IsChecked;
        
        return isWebMFormat ? "" : "[ext=mp4]";
    }

    private string GetVideoAudioExtension()
    {
        bool isWebMFormat = (bool)WebMFormatCheckBox.IsChecked;
        
        return isWebMFormat ? "" : "[ext=m4a]";
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        _cancellationTokenSource?.Cancel();
    }
}
