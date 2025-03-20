using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.AspNetCore.Http;

namespace AzureBlobStorage
{
    internal class BlobService : IBlobService
    {
        readonly BlobServiceClient blobClient = new BlobServiceClient("***");

        public async Task<bool> DeleteBlob(string containerName, string name)
        {
            BlobContainerClient blobContainerClient = blobClient.GetBlobContainerClient(containerName);
            var client = blobContainerClient.GetBlobClient(name);
            return await client.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllBlobs(string containerName)
        {
            BlobContainerClient blobContainerClient = blobClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobsAsync().AsPages(default, 10);
            var blobString = new List<string>();
            await foreach (Page<BlobItem> blobPage in blobs)
            {
                foreach (var blobItem in blobPage.Values)
                {
                    blobString.Add(blobItem.Name);
                }
            }
            return blobString;
        }

        public string GetBlobUrl(string containerName, string name)
        {
            BlobContainerClient blobContainerClient = blobClient.GetBlobContainerClient(containerName);
            var blob = blobContainerClient.GetBlobClient(name);
            return blob.Uri.AbsoluteUri;
        }

        public async Task<byte[]> GetBlobFile(string containerName, string name, FileStream fileStream)
        {
            BlobContainerClient blobContainerClient = blobClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobClient(name);
            byte[]? file = null;
            if (await blobs.ExistsAsync())
            {
                await blobs.DownloadToAsync(fileStream);
                using (MemoryStream ms = new())
                {
                    await blobs.DownloadToAsync(ms);
                    file = ms.ToArray();
                }
            }
            return file;
        }

        public async Task<bool> UploadBlob(string containerName, string name, IFormFile file)
        {
            BlobContainerClient blobContainerClient = blobClient.GetBlobContainerClient(containerName);
            var blobs = blobContainerClient.GetBlobClient(name);
            var httpHeaders = new BlobHttpHeaders()
            {
                ContentType = file.ContentType,
            };
            var result = await blobs.UploadAsync(file.OpenReadStream(), httpHeaders);
            if (result != null)
            {
                return true;
            }
            return false;
        }
    }
}
