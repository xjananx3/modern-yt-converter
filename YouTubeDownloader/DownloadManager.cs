using System.Diagnostics;

namespace YouTubeDownloader;

public class DownloadManager
{
    public async Task StartDownloadAsync(string arguments)
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
}
