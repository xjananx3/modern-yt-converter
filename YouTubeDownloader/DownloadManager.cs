using System.Diagnostics;

namespace YouTubeDownloader;

public class DownloadManager
{
    public async Task StartDownloadAsync(string arguments, CancellationToken cancellationToken)
    {
        
        using var process = new Process();
        process.StartInfo = new ProcessStartInfo
        {
            FileName = "yt-dlp",
            Arguments = arguments,
            CreateNoWindow = true,
            RedirectStandardError = true,
            UseShellExecute = false
        };
        process.EnableRaisingEvents = true;

        process.Start();
        await process.WaitForExitAsync(cancellationToken);

        if (cancellationToken.IsCancellationRequested)
        {
            process.Kill(true);
            cancellationToken.ThrowIfCancellationRequested();
        }
        
    }
}
