using CsvHelper;
using IFPS.Factory.Domain.Exceptions;
using IFPS.Factory.Domain.FileHandling;
using IFPS.Factory.Domain.Model;
using IFPS.Factory.Domain.Repositories;
using IFPS.Factory.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IFPS.Factory.Domain.Services.Implementations
{
    public class FileHandlerService : IFileHandlerService
    {
        private readonly IFileStorage storage;
        private readonly IImageRepository imageRepository;

        public FileHandlerService(IFileStorage storage,
            IImageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
            this.storage = storage;
        }

        public async Task<(string ContainerName, string FileName)> CopyFileFromContainer(string sourceContainer, string sourceFileName, string destinationContainer)
        {
            var extension = Path.GetExtension(sourceFileName);
            ValidateUpload(destinationContainer, extension);

            using (var stream = new MemoryStream())
            {
                await storage.DownloadFile(stream, sourceContainer, sourceFileName);

                var fileName = Guid.NewGuid() + extension;

                await storage.SaveFileAsync(stream, destinationContainer, fileName, FileContainerProvider.ContainerSettings[destinationContainer].IsPublic);

                return (destinationContainer, fileName);
            }
        }

        public async Task DownloadFromContainer(Stream stream, string fileName, string containerName)
        {
            ValidateContainerName(containerName);

            await storage.DownloadFile(stream, containerName, fileName);
        }

        public async Task<string> ExportCsv<T>(List<T> datas, string name, Stream stream)
        {
            var fileName = name + ".csv";
            var writer = new StreamWriter(stream, new UTF8Encoding(true));
            var csv = new CsvWriter(writer);

            csv.Configuration.Delimiter = ";";

            csv.WriteHeader(typeof(T));
            await csv.NextRecordAsync();
            foreach (var item in datas)
            {
                csv.WriteRecord(item);
                await csv.NextRecordAsync();
            }

            await writer.FlushAsync();

            stream.Position = 0;
            return fileName;
        }

        public string GetFileUrl(string containerName, string fileName)
        {
            ValidateContainerName(containerName);

            if (!FileContainerProvider.ContainerSettings[containerName].IsPublic)
            {
                throw new ArgumentException($"The container is not public hence it has no Url.", nameof(containerName));
            }

            return storage.GetFileUrl(containerName, fileName);
        }

        public (byte[] RawData, string Url) GetImage(string containerName, string fileName)
        {
            var url = GetFileUrl(containerName, fileName);
            return (System.IO.File.ReadAllBytes(storage.GetFileFullPath(containerName, fileName)), url);
        }

        public async Task<Guid> InsertImage(string containerName, string fileName)
        {
            var newImage = new Image(fileName, Path.GetExtension(fileName), containerName);
            newImage.ThumbnailName = storage.SaveThumbnailImage(containerName, fileName);

            await imageRepository.InsertAsync(newImage);

            return newImage.Id;
        }

        public Task<Image> UpdateImage(Guid id, string containerName, string fileName)
        {
            var image = imageRepository.Single(ent => ent.Id == id);
            image.ThumbnailName = storage.SaveThumbnailImage(containerName, fileName);
            image.ContainerName = containerName;
            image.Extension = Path.GetExtension(fileName);
            image.FileName = fileName;

            return imageRepository.UpdateAsync(image);
        }

        public async Task<(string ContainerName, string FileName)> UploadFromByteArrayAsync(byte[] fileBytes, string containerName, string extension)
        {
            ValidateUpload(containerName, extension);

            var tempfile = Path.GetTempFileName();
            try
            {
                using (var stream = System.IO.File.Create(tempfile))
                {
                    Stream byteStream = new MemoryStream(fileBytes);
                    var fileName = Guid.NewGuid() + extension;

                    await storage.SaveFileAsync(byteStream, containerName, fileName, FileContainerProvider.ContainerSettings[containerName].IsPublic);

                    return (containerName, fileName);
                }
            }
            finally
            {
                System.IO.File.Delete(tempfile);
            }
        }

        public async Task<(string ContainerName, string FileName)> UploadFromStreamAsync(Stream stream, string containerName, string extension)
        {
            ValidateUpload(containerName, extension);

            var fileName = Guid.NewGuid() + extension;
            var fullPath = await storage.SaveFileAsync(stream, containerName, fileName, FileContainerProvider.ContainerSettings[containerName].IsPublic);

            return (containerName, fileName);
        }

        private void ValidateContainerName(string containerName)
        {
            if (string.IsNullOrWhiteSpace(containerName))
            {
                throw new ArgumentNullException($"The {nameof(containerName)} can not be null.");
            }

            if (!FileContainerProvider.ContainerSettings.ContainsKey(containerName))
            {
                throw new ArgumentException($"Invalid container name: {containerName}") { Source = nameof(containerName) };
            }
        }

        private void ValidateUpload(string containerName, string extension)
        {
            ValidateContainerName(containerName);

            if (!FileContainerProvider.ContainerSettings[containerName].Extensions.Contains(extension, StringComparer.InvariantCultureIgnoreCase))
            {
                throw new IFPSDomainException("Invalid file extension.") { Source = nameof(extension) };
            }
        }
    }
}