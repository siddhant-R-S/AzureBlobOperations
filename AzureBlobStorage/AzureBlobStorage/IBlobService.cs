using Microsoft.AspNetCore.Http;

namespace AzureBlobStorage
{
    internal interface IBlobService
    {
        Task<List<string>> GetAllBlobs(string containerName);
        Task<bool> UploadBlob(string containerName, string name, IFormFile file);
        Task<bool> DeleteBlob(string containerName, string name);
        string GetBlobUrl(string containerName, string name);
        Task<byte[]> GetBlobFile(string containerName, string name, FileStream fileStream);

    }
}
