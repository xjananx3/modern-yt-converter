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
        string videoExtension = GetVideoExtension();
        string videoAudioExtension = GetVideoAudioExtension();
        
        return quality switch
        {
            "480p" => string.Format(Format480p, videoExtension, videoAudioExtension, videoExtension),
            "720p" => string.Format(Format720p, videoExtension, videoAudioExtension, videoExtension),
            "1080p" => string.Format(Format1080p, videoExtension, videoAudioExtension, videoExtension),
            "Best" => string.Format(FormatBest, videoExtension, videoAudioExtension, videoExtension),
            _ => string.Empty
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
    
    private const string Format480p = "bestvideo{0}[height<=480]+bestaudio{1}/best{2}[height<=480]";
    private const string Format720p = "bestvideo{0}[height<=720]+bestaudio{1}/best{2}[height<=720]";
    private const string Format1080p = "bestvideo{0}[height<=1080]+bestaudio{1}/best{2}[height<=1080]";
    private const string FormatBest = "bestvideo{0}+bestaudio{1}/best{2}/best";
}
