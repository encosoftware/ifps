using IFPS.Sales.Domain.FileHandling;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System.IO;
using System.Threading.Tasks;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.PixelFormats;
using System;
using IFPS.Sales.Domain.Model;
using Image = SixLabors.ImageSharp.Image;
using Newtonsoft.Json;

namespace IFPS.Sales.EF.FileHandling
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly string _link;
        private readonly LocalFileStorageConfiguration _config;
        private readonly string _uploadFolder;
        private readonly string _thumnailPrefix;

        public LocalFileStorage(IOptions<LocalFileStorageConfiguration> options,
            IHostingEnvironment hostingEnvironment)
        {
            _config = options.Value;
            _link = InitLink();
            _uploadFolder = Path.Combine(hostingEnvironment.ContentRootPath, "AppData");
            _thumnailPrefix = "thumbnail_";
        }

        private string InitLink()
        {
            string link = _config.BaseUrl;
            return link;
        }

        public async Task DownloadFile(Stream stream, string containerName, string fileName)
        {
            using (var fileStream = new FileStream(Path.Combine(_uploadFolder, containerName, fileName), FileMode.Open,FileAccess.Read,FileShare.Read,4096,true))
            {
                await fileStream.CopyToAsync(stream);
            }
        }

        public string GetFileUrl(string container, string fileName)
        {
            return Path.Combine(_link, _uploadFolder, container, fileName);
        }

        public async Task<string> SaveFileAsync(Stream stream, string containerName, string fileName, bool isPublic)
        {
            var directory = Path.Combine(_uploadFolder, containerName);

            var fullPath = Path.Combine(directory, fileName);
            Directory.CreateDirectory(directory);

            using (var fileStream = new FileStream(fullPath, FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }

            return fullPath;
        }

        public async Task<string> SaveJsonFileAsync<T>(T objectToSerialize, string containerName, string fileName, bool isPublic)
        {
            var directory = Path.Combine(_uploadFolder, containerName);

            var fullPath = Path.Combine(directory, fileName);
            Directory.CreateDirectory(directory);

            using (var fileStream = System.IO.File.CreateText(fullPath))
            {
                var serializer = new JsonSerializer();
                serializer.Serialize(fileStream, objectToSerialize);
            }

            return fullPath;
        }

        public string SaveThumbnailImage(string containerName, string fileName)
        {
            var directory = Path.Combine(_uploadFolder, containerName);

            var fullPath = Path.Combine(directory, fileName);
            var thumbnailPath = Path.Combine(directory, _thumnailPrefix + fileName);
            try
            {
                using (Image<Rgba32> image = Image.Load(fullPath))
                {
                    image.Mutate(x => x
                         .Resize(image.Width / 2, image.Height / 2));
                    image.Save(thumbnailPath);
                }
            }
            catch (Exception e)
            {
                throw new FileLoadException($"Failed to create thumbnail. {e}");
            }

            return _thumnailPrefix + fileName;
        }

        public string GetFileFullPath(string containerName, string fileName)
        {
            return Path.Combine(_uploadFolder, containerName, fileName);
        }
    }
}