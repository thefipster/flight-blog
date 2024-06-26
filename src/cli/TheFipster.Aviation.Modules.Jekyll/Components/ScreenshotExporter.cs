﻿using Thefipster.Aviation.Modules.Screenshots.Components;
using TheFipster.Aviation.CoreCli;
using TheFipster.Aviation.Domain.Enums;

namespace TheFipster.Aviation.Modules.Jekyll.Components
{
    public class ScreenshotExporter
    {
        private const int PreviewWidth = 400;
        private const int PreviewHeight = 300;
        private const string PreviewFolder = "tn";

        private readonly FlightFileScanner scanner;
        private readonly FlightMeta meta;
        private readonly ImageResizer resizer;
        private readonly ImageConverter converter;

        public ScreenshotExporter()
        {
            scanner = new FlightFileScanner();
            meta = new FlightMeta();
            resizer = new ImageResizer();
            converter = new ImageConverter();
        }

        internal void GenerateImages(string flightFolder, string captureFolder)
        {
            var screenshotFiles = scanner.GetFiles(flightFolder, FileTypes.Screenshot);
            if (!screenshotFiles.Any())
                return;

            var flightNo = meta.GetLeg(flightFolder);

            var flightCaptures = Path.Combine(captureFolder, flightNo.ToString());
            if (!Directory.Exists(flightCaptures))
                Directory.CreateDirectory(flightCaptures);

            var flightPreviews = Path.Combine(flightCaptures, PreviewFolder);
            if (!Directory.Exists(flightPreviews))
                Directory.CreateDirectory(flightPreviews);

            foreach (var screenshotFile in screenshotFiles)
            {
                string newImageFilename = GetFinalImageNameFromFilepath(screenshotFile);

                // convert to jpg
                var newImageFile = Path.Combine(flightCaptures, newImageFilename);
                converter.PngToJpg(screenshotFile, newImageFile);

                // generate preview
                var newPreviewFile = Path.Combine(flightPreviews, newImageFilename);
                resizer.Resize(newImageFile, newPreviewFile, PreviewWidth, PreviewHeight);
            }
        }

        public void CopyLanding(string folder, string replayFolder, bool overwrite = false)
        {
            try
            {
                var landingFile = scanner.GetFile(folder, FileTypes.ReplayLanding);
                var flightNo = meta.GetLeg(folder);

                var filename = $"{flightNo}-landing.mp4";
                var newFile = Path.Combine(replayFolder, filename);

                if (File.Exists(newFile) && !overwrite)
                    return;

                if (!Directory.Exists(replayFolder))
                    Directory.CreateDirectory(replayFolder);

                File.Move(landingFile, newFile, overwrite);
            }
            catch (Exception)
            {
                // lets just skip the landing replay
            }
        }

        public static string GetFinalImageNameFromFilename(string filename)
        {
            var nameParts = filename.Split(" - ");
            var lastPart = nameParts.Last();
            if (!string.IsNullOrEmpty(Path.GetExtension(filename)))
                lastPart = lastPart.Replace(Path.GetExtension(filename), string.Empty);

            int imageNo = int.Parse(lastPart);
            var newFilename = imageNo.ToString("d3") + ".jpg";
            return newFilename;
        }

        public static string GetFinalImageNameFromFilepath(string filepath)
        {
            var filename = Path.GetFileName(filepath);
            return GetFinalImageNameFromFilename(filename);
        }

        public void Export(string flightFolder, string screenshotFolder, bool overwrite = false)
        {
            var flightNumber = new FlightMeta().GetLeg(flightFolder);
            var imageFolder = Path.Combine(screenshotFolder, flightNumber.ToString());
            var tnFolder = Path.Combine(screenshotFolder, flightNumber.ToString(), "tn");
            exportScreenshots(flightFolder, imageFolder, overwrite);
            exportThumbnails(flightFolder, tnFolder, overwrite);
        }

        private void exportScreenshots(string flightFolder, string screenshotFolder, bool overwrite = false)
        {
            var screenshots = new FlightFileScanner().GetFiles(flightFolder, FileTypes.Image);

            if (screenshots.Any() && !Directory.Exists(screenshotFolder))
                Directory.CreateDirectory(screenshotFolder);

            foreach (var file in screenshots)
            {
                var filename = Path.GetFileName(file);
                filename = filename
                    .Replace(" 1.jpg", " 01.jpg")
                    .Replace(" 2.jpg", " 02.jpg")
                    .Replace(" 3.jpg", " 03.jpg")
                    .Replace(" 4.jpg", " 04.jpg")
                    .Replace(" 5.jpg", " 05.jpg")
                    .Replace(" 6.jpg", " 06.jpg")
                    .Replace(" 7.jpg", " 07.jpg")
                    .Replace(" 8.jpg", " 08.jpg")
                    .Replace(" 9.jpg", " 09.jpg")
                    .Replace(" ", string.Empty);
                var newPath = Path.Combine(screenshotFolder, filename);

                if (File.Exists(newPath) && !overwrite)
                    continue;

                File.Copy(file, newPath);
            }
        }

        private void exportThumbnails(string flightFolder, string thumbnailFolder, bool overwrite = false)
        {
            var thumbnails = new FlightFileScanner().GetFiles(flightFolder, FileTypes.Preview);

            if (thumbnails.Any() && !Directory.Exists(thumbnailFolder))
                Directory.CreateDirectory(thumbnailFolder);

            foreach (var file in thumbnails)
            {
                var filename = Path.GetFileName(file);
                filename = filename
                    .Replace(" 1.jpg", " 01.jpg")
                    .Replace(" 2.jpg", " 02.jpg")
                    .Replace(" 3.jpg", " 03.jpg")
                    .Replace(" 4.jpg", " 04.jpg")
                    .Replace(" 5.jpg", " 05.jpg")
                    .Replace(" 6.jpg", " 06.jpg")
                    .Replace(" 7.jpg", " 07.jpg")
                    .Replace(" 8.jpg", " 08.jpg")
                    .Replace(" 9.jpg", " 09.jpg")
                    .Replace(" ", string.Empty);
                var newPath = Path.Combine(thumbnailFolder, filename);

                if (File.Exists(newPath) && !overwrite)
                    continue;

                File.Copy(file, newPath);
            }
        }
    }
}
