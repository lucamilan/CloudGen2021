using Azure.Storage.Blobs;
using Dapr.Cqrs.Core.ConnectionStrings;
using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Dapr.Cqrs.Common.Models.Write;

namespace Dapr.Cqrs.Processor.Search.Services
{
    public class AzureBlobManagement
    {
        private readonly BlobContainerClient _containerClient;

        public AzureBlobManagement(ConnectionStringsRegistry connectionStringsRegistry)
        {
            if (connectionStringsRegistry == null) throw new ArgumentNullException(nameof(connectionStringsRegistry));
            _containerClient = new BlobContainerClient(connectionStringsRegistry.GetAzureBlob(), "search-data");
        }

        public async Task CreateIfNotExistsAsync(Guid id, SensorData data)
        {
            await _containerClient.CreateIfNotExistsAsync();
            var dataPattern = data.RecordedOn.ToString("yyyy/MM/dd/HHmm");
            var blobName = $"{data.Plant}/{data.Location}/{data.Tag}/{dataPattern}/{data.RecordedOn.ToString("ss")}-{id:N}.json";
            var blobClient = _containerClient.GetBlobClient(blobName);
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

            await blobClient.UploadAsync(ms);
        }
    }
}