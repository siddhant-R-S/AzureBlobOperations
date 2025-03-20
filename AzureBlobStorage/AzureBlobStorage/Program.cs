// See https://aka.ms/new-console-template for more information
using AzureBlobStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;

public static class Program
{
    private static async Task Main(string[] args)
    {
        IContainerService containerService = new ContainerService();
        await containerService.CreateContainer("testcontainer");

        await GetAllContainers(containerService);

        await containerService.DeleteContainer("testcontainer");

        await UploadFileToBlobStorage();

        await GetAllBlobs();

        GetBlobUrl();

        await GetBlobFile();

        await DeleteBlob();

        Console.ReadLine();
    }

    private static async Task GetAllContainers(IContainerService containerService)
    {
        var containerList = await containerService.GetAllContainers();
        foreach (var container in containerList)
        {
            Console.WriteLine(container);
        }
    }

    private static async Task GetAllBlobs()
    {
        IBlobService blobService = new BlobService();
        var fileList = await blobService.GetAllBlobs("vensureoneqablob");
        foreach (var item in fileList)
        {
            Console.WriteLine(item);
        }
    }

    private static void GetBlobUrl()
    {
        IBlobService blobService = new BlobService();
        var url = blobService.GetBlobUrl("vensureoneqablob", "BrokerLogos/01102023085519AM24_vensure.png");
        Console.WriteLine(url);
    }

    private static async Task GetBlobFile()
    {
        IBlobService blobService = new BlobService();
        FileStream fileStream = File.OpenWrite($"C:\\Users\\siddhant.shivani\\Downloads\\Blob\\Azure AD B2C Authentication.pdf");
        var result = await blobService.GetBlobFile("vensureoneqablob", "ClientData/1/Azure AD B2C Authentication.pdf", fileStream);

        Console.WriteLine(result);

        fileStream.Close();
    }

    static async Task UploadFileToBlobStorage()
    {
        IBlobService blobService = new BlobService();
        using (var stream = File.OpenRead("C:\\Users\\siddhant.shivani\\Downloads\\Documents\\Azure AD B2C Authentication.pdf"))
        {
            var file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
            {
                Headers = new HeaderDictionary(),
                ContentType = "application/pdf"
            };
            var result = await blobService.UploadBlob("vensureoneqablob", $"ClientData/1/{file.FileName}", file);
            if (result)
                Console.WriteLine("success");
            else
                Console.WriteLine("failure");
        }
    }

    static async Task DeleteBlob()
    {
        IBlobService blobService = new BlobService();
        var result = await blobService.DeleteBlob("vensureoneqablob", "BrokerLogos/01102023085519AM24_vensure.png");
        if (result)
            Console.WriteLine("success");
        else
            Console.WriteLine("failure");
    }
}