using System.IO;
using System.Threading.Tasks;

namespace IFPS.Sales.Domain.FileHandling
{
    public interface IFileStorage
    {
        Task<string> SaveFileAsync(Stream stream, string containerName, string fileName, bool isPublic);
        string SaveThumbnailImage(string containerName, string fileName);
        Task DownloadFile(Stream stream, string containerName, string fileName);
        string GetFileUrl(string container, string fileName);
        string GetFileFullPath(string containerName, string fileName);
        Task<string> SaveJsonFileAsync<T>(T objectToSerialize, string containerName, string fileName, bool isPublic);
    }
}
