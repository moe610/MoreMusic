﻿using System.Text.RegularExpressions;
using YoutubeExplode;
using YoutubeExplode.Videos.Streams;
using System.Diagnostics;
using MoreMusic.DataLayer;
using MoreMusic.Models;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;
using MoreMusic.DataLayer.Entity;
using Microsoft.EntityFrameworkCore;

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
        public async Task<NewAudioFileImportModel> Uploader(string youtubeUrl)
        {
            string title, author;
            string audioFilesBasePath = _configuration["AudioFilesBasePath"];

            var youtube = new YoutubeClient();
            try
            {
                // You can specify either video ID or URL
                var video = await youtube.Videos.GetAsync(youtubeUrl);
                title = video.Title;
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
                await youtube.Videos.Streams.DownloadAsync(audioStreamInfo, filePath);

                return ConvertToAAC(filePath,audioFilesBasePath);
            }
            catch (Exception ex)
            {
                throw new Exception ("Download failed: " + ex.Message);
            }
        }

        private NewAudioFileImportModel ConvertToAAC(string filePath, string basePath)
        {
            string inputFilePath = filePath;
            string fileName = Path.GetFileName(inputFilePath);

            string outputFileName = Path.ChangeExtension(fileName, ".aac");
            string outputFilePath = Path.Combine(@$"{basePath}", outputFileName);

            try
            {
                string ffmpegPath = @"C:\PATH_Programs\ffmpeg-6.0-full_build\bin\ffmpeg.exe";
                string command = $"-i \"{inputFilePath}\" \"{outputFilePath}\"";

                ProcessStartInfo processStartInfo = new ProcessStartInfo(ffmpegPath)
                {
                    Arguments = command,
                    UseShellExecute = false,
                    RedirectStandardError = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                };

                Process process = new Process
                {
                    StartInfo = processStartInfo,
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

                NewAudioFileImportModel importDetails = new NewAudioFileImportModel()
                {
                    fileName = outputFileName,
                    filePath = basePath,
                    fileType = Path.GetExtension(outputFileName).TrimStart('.'),
                    serverId = 1
                };

                return importDetails;
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred: {ex.Message}");
            }
        }

        public async Task InsertAudioFileToDb(NewAudioFileImportModel importDetails)
        {
            try
            {
                var audioFiles = _dbContext.AudioFiles;

                string filePath = Path.Combine(importDetails.filePath, importDetails.fileName);

                using (var dbTransaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        AudioFiles newAudioFiles = new AudioFiles()
                        {
                            FileName = importDetails.fileName,
                            FilePath = filePath,
                            Title = importDetails.fileName,
                            FileType = importDetails.fileType,
                            ServerId = 1
                        };

                        audioFiles.Add(newAudioFiles);
                        _dbContext.SaveChanges();
                        dbTransaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        dbTransaction.Rollback();     
                        throw new Exception(ex.Message);          
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
