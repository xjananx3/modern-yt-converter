# 🎥 YouTube Downloader

A simple, modern and powerful YouTube Downloader built with XAML and C# on Uno Platform that allows you to download videos and audios in various formats and qualities. The application supports multiple audio and video formats and provides a user-friendly interface.

## 🚀 Features

- **Video Download**: Download videos in various resolutions such as 480p, 720p, 1080p, or the best available quality.
- **Audio Download**: Extract audio in formats like `opus`, `mp3`, and `m4a`.
- **Choose Save Location**: Easily select the destination folder for the downloaded files.
- **Cancel Function**: Cancel the download process at any time.

## 🛠️ Installation

### Prerequisites

- .NET 8
- Linux (e.g. Ubuntu, Arch, Debian or any other distribution that supports Skia and WinUI)
- [yt-dlp](https://github.com/yt-dlp/yt-dlp)
- [FFmpeg](https://ffmpeg.org/)

### Install Dependencies

Ensure all required packages are installed to run the application. On Arch Linux, you might do this as follows:

```bash
sudo pacman -S dotnet-sdk ffmpeg yt-dlp
```
### Clone the project

```bash
git clone https://github.com/xjananx3/YouTubeDownloader.git
cd YouTubeDownloader
```
### Build and Run

```bash
dotnet build
dotnet run
```

## 📄 Usage
1. **Enter YouTube Link**: Paste the YouTube link into the text field.
2. **Choose Save Location**: Click "Browse" to select the folder where the file will be saved.
3. **Select Format**: Choose between audio or video and select the desired quality.
4. **Start Download**: Click the "Download" button to start downloading.

## 🧩 Technologies
- **.NET 8**
- **XAML**
- **Uno Platform** (https://platform.uno/)
- **yt-dlp**: Used for downloading and converting YouTube videos and audios.
- **FFmpeg**: Required for processing and converting audio files.
- **Skia**: Used for the graphical interface on Linux.

## 🛡️ License
This project is licensed under the MIT License. See the LICENSE file for more details.

## 🙏 Acknowledgements
A big thank you to all the developers and open-source projects that made this work possible, especially `yt-dlp` for their excellent download tool, and `ffmpeg` for handling audio/video processing.


