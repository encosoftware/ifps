using IFPS.Sales.Domain.Model;
using System;
using System.IO;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.Services.Interfaces
{
    public interface IFileHandlerService
    {
        Task<(string ContainerName, string FileName)> UploadFromByteArrayAsync(byte[] fileBytes, string containerName, string extension);
        Task<(string ContainerName, string FileName)> UploadFromStreamAsync(Stream stream, string containerName, string extension);
        Task<(string ContainerName, string FileName)> CopyFileFromContainer(string sourceContainer, string sourceFileName, string destinationContainer);
        Task DownloadFromContainer(Stream stream, string fileName, string containerName);
        string GetFileUrl(string containerName, string fileName);
        Task<Guid> InsertImage(string containerName, string fileName);
        Task<Image> UpdateImage(Guid id, string containerName, string fileName);        
        (byte[] RawData, string Url) GetImage(string containerName, string fileName);
        bool IsFileExist(string containerName, string fileName);
        string GetFileFullPath(string containerName, string fileName);
        void RenameFile(string containerName, string oldFileName, string newFileName);
        Task<(string ContainerName, string FileName)> UploadJsonFileAsync<T>(T objectToSerialize, string containerName, string fileName);
    }
}
