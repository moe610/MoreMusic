using System.Text.RegularExpressions;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using System.Diagnostics;
using MoreMusic.DataLayer;
using MoreMusic.Models;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace MoreMusic.Services
{
    public class UploadMusicService
    {
        private readonly MusicDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public UploadMusicService(MusicDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public async void Downloader(string youtubeUrl)
        {
            string title, author;
            string audioFilesBasePath = _configuration["AudioFilesBasePath"];

            var youtube = new YoutubeClient();
            try
            {
                // You can specify either video ID or URL
                var video = await youtube.Videos.GetAsync(youtubeUrl);
                title = video.Title; // "Collections - Blender 2.80 Fundamentals"
                if (!title.Contains('-'))
                    title = Regex.Replace(title, @"[^0-9a-zA-Z]+", " ");
                else
                {
                    author = title.Substring(0, title.IndexOf("-"));
                    title = title.Substring(title.LastIndexOf('-') + 1);
                    author = Regex.Replace(author, @"[^0-9a-zA-Z]+", " ");
                    title = Regex.Replace(title, @"[^0-9a-zA-Z]+", " ");
                    title = author + " - " + title;
                }

                var filePath = @$"{audioFilesBasePath}{ title}.mp3";
                var streamManifest = await youtube.Videos.Streams.GetManifestAsync(youtubeUrl);
                var audioStreamInfo = streamManifest.GetAudioOnlyStreams().GetWithHighestBitrate();
                // Download the stream to a file
                await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, filePath);

                ConvertToAAC(filePath,audioFilesBasePath);
            }
            catch (Exception ex)
            {
                throw new Exception ("Download failed: " + ex.Message);
            }
        }

        private void ConvertToAAC(string filePath, string basePath)
        {
            string inputFilePath = filePath;
            string fileName = Path.GetFileName(inputFilePath);

            string outputFileName = Path.ChangeExtension(fileName, ".aac"); // Change the extension to .aac
            string outputFilePath = Path.Combine(@$"{basePath}", outputFileName); // Construct the output file path

            try
            {
                // Ensure FFmpeg executable path is set correctly
                string ffmpegPath = @"C:\PATH_Programs\ffmpeg-6.0-full_build\bin\ffmpeg.exe";
                // Construct the FFmpeg command
                string command = $"-i \"{inputFilePath}\" \"{outputFilePath}\"";

                ProcessStartInfo psi = new ProcessStartInfo(ffmpegPath)
                {
                    Arguments = command,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                };

                Process process = new Process
                {
                    StartInfo = psi,
                    EnableRaisingEvents = true,
                };

                string outputData = "";

                // Handle standard output
                process.OutputDataReceived += (s, args) =>
                {
                    if (!string.IsNullOrEmpty(args.Data))
                    {
                        outputData += args.Data + Environment.NewLine;
                    }
                };

                process.Exited += (s, args) =>
                {
                    // remove mp3 files created
                    File.Delete(inputFilePath);

                };

                process.Start();
                process.BeginOutputReadLine();
                process.WaitForExit();

                InsertAudioFileToDb(outputFileName, basePath);
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }

        private void InsertAudioFileToDb(string fileName, string basePath)
        {
            try
            {
                var audioFiles = _dbContext.audioFiles;

                // Construct the full file path
                string filePath = Path.Combine(basePath, fileName);
                using (var dbTransaction = _dbContext.Database.BeginTransaction())
                {
                    AudioFiles newAudioFiles = new AudioFiles()
                    {
                        FileName = fileName,
                        FilePath = filePath,
                        Title = fileName
                    };

                    audioFiles.Add(newAudioFiles);
                }  
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
