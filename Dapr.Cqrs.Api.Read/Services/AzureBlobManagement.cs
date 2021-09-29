using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Dapr.Cqrs.Common.Models.Read;
using Dapr.Cqrs.Core.ConnectionStrings;

namespace Dapr.Cqrs.Api.Read.Services {
    public class AzureBlobManagement {
        private readonly BlobContainerClient _rawContainerClient;
        private readonly BlobContainerClient _searchContainerClient;

        public AzureBlobManagement (ConnectionStringsRegistry connectionStringsRegistry) {
            if (connectionStringsRegistry == null) throw new ArgumentNullException (nameof (connectionStringsRegistry));
            _rawContainerClient = new BlobContainerClient (connectionStringsRegistry.GetAzureBlob (), "raw-data");
            _searchContainerClient = new BlobContainerClient (connectionStringsRegistry.GetAzureBlob (), "search-data");
        }

        public async Task<IEnumerable<string>> GetListOfDirectoriesAsync (String initPrefix) {

            await _searchContainerClient.CreateIfNotExistsAsync ();

            Queue<string> prefixes = new Queue<string> ();
            prefixes.Enqueue (initPrefix);
            List<string> directoryNames = new List<string> ();
            do {
                string prefix = prefixes.Dequeue ();
                bool isLast = prefix.Split ("/").Length == 8;
                await
                foreach (BlobHierarchyItem blobHierarchyItem in _searchContainerClient.GetBlobsByHierarchyAsync (prefix: prefix, delimiter: "/")) {
                    if (blobHierarchyItem.IsPrefix) {
                        //directoryNames.Add (blobHierarchyItem.Prefix);
                        prefixes.Enqueue (blobHierarchyItem.Prefix);
                    } else {
                        directoryNames.Add (blobHierarchyItem.Blob.Name);
                    }
                }
            } while (prefixes.Count > 0);

            return directoryNames;
        }

        public async Task<RawDataDto> DownloadContentAsync (Guid id) {
            await _rawContainerClient.CreateIfNotExistsAsync ();

            var blobClient = _rawContainerClient.GetBlobClient ($"{id:N}.json");

            if (await blobClient.ExistsAsync () == false) {
                return null;
            }

            await using var stream = new MemoryStream ();
            await blobClient.DownloadToAsync (stream);
            var props = await blobClient.GetPropertiesAsync ();
            stream.Position = 0;
            using var sr = new StreamReader (stream);
            var body = await sr.ReadToEndAsync ();
            return new RawDataDto {
                Id = id,
                    Json = body,
                    Type = props.Value.Metadata["AssemblyQualifiedName"],
                    CreatedOn = props.Value.CreatedOn
            };
        }
    }
}