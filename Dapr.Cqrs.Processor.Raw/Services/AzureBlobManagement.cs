using Azure.Storage.Blobs;
using Dapr.Cqrs.Core.ConnectionStrings;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Dapr.Cqrs.Common.Models.Write;

namespace Dapr.Cqrs.Processor.Raw.Services
{
    public class AzureBlobManagement
    {
        private readonly BlobContainerClient _containerClient;

        public AzureBlobManagement(ConnectionStringsRegistry connectionStringsRegistry)
        {
            if (connectionStringsRegistry == null) throw new ArgumentNullException(nameof(connectionStringsRegistry));
            _containerClient = new BlobContainerClient(connectionStringsRegistry.GetAzureBlob(), "raw-data");
        }

        public async Task CreateIfNotExistsAsync(Guid id, SensorData data)
        {
            await _containerClient.CreateIfNotExistsAsync();
            var blobClient = _containerClient.GetBlobClient($"{id:N}.json");

            await using var ms = new MemoryStream();
            await JsonSerializer.SerializeAsync(ms, data, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                PropertyNameCaseInsensitive = true
            });
            ms.Position = 0;

            if (await blobClient.ExistsAsync())
            {
                return;
            }

            await blobClient.UploadAsync(ms, metadata: new Dictionary<string, string>
            {
                {"AssemblyQualifiedName", data.GetType().AssemblyQualifiedName}
            });
        }
    }
}