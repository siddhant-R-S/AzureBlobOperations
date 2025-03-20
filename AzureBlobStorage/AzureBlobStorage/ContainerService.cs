using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace AzureBlobStorage
{
    public class ContainerService : IContainerService
    {
        readonly BlobServiceClient blobClient = new BlobServiceClient("***");
        public async Task CreateContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = blobClient.GetBlobContainerClient(containerName);
            await blobContainerClient.CreateIfNotExistsAsync();
            //await blobContainerClient.CreateIfNotExistsAsync(PublicAccessType.BlobContainer);
        }

        public async Task DeleteContainer(string containerName)
        {
            BlobContainerClient blobContainerClient = blobClient.GetBlobContainerClient(containerName);
            await blobContainerClient.DeleteIfExistsAsync();
        }

        public async Task<List<string>> GetAllContainers()
        {
            List<string> containers = new();
            await foreach (BlobContainerItem blobContainerItem in blobClient.GetBlobContainersAsync())
            {
                containers.Add(blobContainerItem.Name);
            }
            return containers;
        }
    }
}
