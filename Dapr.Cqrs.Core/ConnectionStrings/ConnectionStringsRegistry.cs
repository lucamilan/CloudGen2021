using Microsoft.Extensions.Configuration;
using System;

namespace Dapr.Cqrs.Core.ConnectionStrings
{
    public sealed class ConnectionStringsRegistry
    {
        private readonly IConfiguration _configuration;

        public ConnectionStringsRegistry(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public string GetAzureBlob()
        {
            return _configuration.GetConnectionString("azurite", "blobs");
        }

        public string GetAzureTables()
        {
            return _configuration.GetConnectionString("azurite", "tables");
        }

        public string GetSqlServer()
        {
            return _configuration.GetConnectionString("sqlserver");
        }

        public string GetRedis()
        {
            return _configuration.GetConnectionString("redis");
        }
    }
}