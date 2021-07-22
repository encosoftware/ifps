using IFPS.Sales.Domain.Exceptions;
using IFPS.Sales.Domain.FileHandling;
using IFPS.Sales.Domain.Model;
using IFPS.Sales.Domain.Repositories;
using IFPS.Sales.Domain.Services.Interfaces;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Implementations
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

        public async Task<(string ContainerName, string FileName)> UploadJsonFileAsync<T>(T objectToSerialize, string containerName, string fileName)
        {
            ValidateUpload(containerName, ".json");

            var fullPath = await storage.SaveJsonFileAsync(objectToSerialize, containerName, fileName, FileContainerProvider.ContainerSettings[containerName].IsPublic);

            return (containerName, fileName);
        }

        public async Task<(string ContainerName, string FileName)> UploadFromStreamAsync(Stream stream, string containerName, string extension)
        {
            ValidateUpload(containerName, extension);

            var fileName = Guid.NewGuid() + extension;
            var fullPath = await storage.SaveFileAsync(stream, containerName, fileName, FileContainerProvider.ContainerSettings[containerName].IsPublic);
            
            return (containerName, fileName);
        }
        
        public bool IsFileExist(string containerName, string fileName)
        {
            ValidateContainerName(containerName);
            var path = storage.GetFileFullPath(containerName, fileName);
            return System.IO.File.Exists(path);
        }

        public string GetFileFullPath(string containerName, string fileName)
        {
            ValidateContainerName(containerName);
            return storage.GetFileFullPath(containerName, fileName);
        }

        public void RenameFile(string containerName, string oldFileName, string newFileName )
        {
            ValidateContainerName(containerName);
            var path = storage.GetFileFullPath(containerName, oldFileName);

            if(!System.IO.File.Exists(path))
            {
                throw new ArgumentException($"The {oldFileName} file not exist.");
            }

            System.IO.File.Move(path, path.Replace(oldFileName, newFileName));
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
                throw new IFPSDomainException("Unsupported file extension.") { Source = nameof(extension) };
            }
        }        
    }
}
