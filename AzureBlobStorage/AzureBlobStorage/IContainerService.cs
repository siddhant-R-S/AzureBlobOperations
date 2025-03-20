using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureBlobStorage
{
    public interface IContainerService
    {
        Task CreateContainer(string containerName);
        Task DeleteContainer(string containerName);
        Task<List<string>> GetAllContainers();
    }
}
