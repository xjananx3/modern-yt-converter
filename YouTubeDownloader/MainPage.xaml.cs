using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;
using Windows.Services.Maps.OfflineMaps;
using YouTubeDownloader.Assets;

namespace YouTubeDownloader;

public sealed partial class MainPage : Page
{
    private readonly FileProvider _fileProvider = new();
    public MainPage()
    {
        this.InitializeComponent();
    }

    private async void BrowseButton_Click(object sender, RoutedEventArgs e)
    {
        var folderPath = await _fileProvider.ChooseDownloadFolderAsync();
        FolderPathTextBox.Text = folderPath;
    }

    private async void DownloadButton_Click(object sender, RoutedEventArgs e)
    {
        await StartDownload();
    }

    private async Task StartDownload()
    {
        var arguments = GetArguments();
        await DownloadFileProcess(arguments);
    }

    private async Task DownloadFileProcess(string arguments)
    {
        Process ffmProcess = new Process();
        ffmProcess.StartInfo.FileName = "yt-dlp";
        ffmProcess.StartInfo.Arguments = arguments;
        ffmProcess.StartInfo.CreateNoWindow = true;
        ffmProcess.StartInfo.RedirectStandardError = true;
        ffmProcess.StartInfo.UseShellExecute = false;
        ffmProcess.EnableRaisingEvents = true;
        
        ffmProcess.Start();

        ffmProcess.BeginErrorReadLine();
        ffmProcess.BeginOutputReadLine();
        await ffmProcess.WaitForExitAsync();

        ffmProcess.Close();
    }
    

    private string GetArguments()
    {
        string folderArg = @"-o ";
        string filename = @"%(title)s.%(ext)s ";
        string url = YouTubeLinkTextBox.Text;

        string fileTypeArg = AudioFormatRadioButton.IsChecked == true ? GetAudioArguments() : GetVideoArguments();


        string arguments = folderArg + FolderPathTextBox.Text + filename + fileTypeArg + url;
        return arguments;
    }

    private string GetAudioArguments()
    {
        string format = ((ComboBoxItem)AudioFormatComboBox.SelectedItem).Content.ToString().ToLower();
        
        return $"-x --audio-format {format} ";
    }

    private string GetVideoArguments()
    {
        string quality = GetQualityArgument();

        return $"-f '{quality}' ";
    }
    
    

    private string GetQualityArgument()
    {
        string quality = ((ComboBoxItem)BitrateComboBox.SelectedItem).Content.ToString();
        
        return quality switch
        {
            "480p" => "best[height<=480]",
            "720p" => "best[height<=720]",
            "1080p" => "-best[height<=1080]",
            _ => ""
        };
    }
}
