using System;
using System.Collections.Generic;
using System.Data;
using System.Text.Json;
using Dapper;
using Dapr.Cqrs.Common;
using Dapr.Cqrs.Common.Exceptions;
using Dapr.Cqrs.Common.Models.Events;
using Dapr.Cqrs.Common.Models.Write;
using Dapr.Cqrs.Core.ConnectionStrings;
using Microsoft.Data.SqlClient;

namespace Dapr.Cqrs.TransactionalOutbox {
    public class TransactionalOutboxService {
        private readonly string _connectionString;

        private readonly string[] _statements = {
            @"UPDATE Outbox SET [Status] = 1, UpdatedOn = GETDATE() OUTPUT inserted.StorageType StorageType, inserted.EventId EventId, inserted.Payload Payload WHERE [Status] = 0 AND UpdatedOn IS NULL;",
            @"DELETE FROM Outbox WHERE StorageType = @StorageType AND EventId = @EventId;",
            @"UPDATE Outbox SET [Status] = 0, Retry = Retry + 1, UpdatedOn = NULL FROM Outbox WHERE [Status] = 1 AND Retry < 1 AND UpdatedOn IS NOT NULL AND DATEDIFF(second, UpdatedOn, GETDATE()) > 15;",
            @"DELETE FROM Outbox WHERE [Status] = 1 AND Retry > 0;"
        };

        public TransactionalOutboxService (ConnectionStringsRegistry connectionStringsRegistry) {
            if (connectionStringsRegistry == null) throw new ArgumentNullException (nameof (connectionStringsRegistry));
            _connectionString = connectionStringsRegistry.GetSqlServer ();
        }

        public IEnumerable < (Guid Id, SensorData Data, StorageType Storage) > GetMessages () {
            using var connection = new SqlConnection (_connectionString);

            OpenConnection (connection);

            var items = connection.Query<OutboxItem> (_statements[0], commandType : CommandType.Text);

            foreach (var item in items)
                yield return (item.EventId, JsonSerializer.Deserialize<SensorData> (item.Payload, MyDefaults.JsonSerializerOptions), item.StorageType);
        }

        private static void OpenConnection (IDbConnection connection) {
            try {
                connection.Open ();
            } catch (Exception) {
                throw new SqlServerNotAvailableException ();
            }
        }

        public bool MarkMessageAsProcessed (SensorDataProcessed @event) {
            using var connection = new SqlConnection (_connectionString);

            OpenConnection (connection);

            var ps = new DynamicParameters ();

            ps.Add ("@EventId", @event.EventId);

            ps.Add ("@StorageType", @event.StorageType);

            return 1 == connection.Execute (_statements[1], ps, commandType : CommandType.Text);
        }

        public int ReSendMessages () {
            using var connection = new SqlConnection (_connectionString);

            OpenConnection (connection);

            return connection.Execute (_statements[2], commandType : CommandType.Text);
        }

        public int RemoveBadMessages () {
            using var connection = new SqlConnection (_connectionString);

            OpenConnection (connection);

            return connection.Execute (_statements[3], commandType : CommandType.Text);
        }
        private class OutboxItem {
            public Guid EventId { get; set; }
            public StorageType StorageType { get; set; }
            public string Payload { get; set; }
        }
    }
}