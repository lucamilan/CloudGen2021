using System;
using System.Data;
using System.Text.Json;
using Dapper;
using Dapr.Cqrs.Common;
using Dapr.Cqrs.Common.Models.Write;
using Dapr.Cqrs.Core.ConnectionStrings;
using Microsoft.Data.SqlClient;

namespace Dapr.Cqrs.Api.Write.Commands {
    public class WriteDataCommand {
        private readonly string _connectionString;

        private readonly string[] _statements = {
            @"
            BEGIN TRAN;
            DECLARE @EventId uniqueidentifier, @Now datetime;
            SET @EventId = NEWID();
            SET @Now = GETDATE();
            insert into SensorData (Id, Location, Plant, Tag, Value, RecordedOn, CreatedOn) values (@EventId, @location, @plant, @tag, @value, @recordedOn, @Now);
            insert into Outbox (EventId, Payload, StorageType, CreatedOn) values ( @EventId, @Payload, 1, @Now);
            insert into Outbox (EventId, Payload, StorageType, CreatedOn) values ( @EventId, @Payload, 2, @Now);
            insert into Outbox (EventId, Payload, StorageType, CreatedOn) values ( @EventId, @Payload, 3, @Now);
            COMMIT TRAN;
        "
        };

        public WriteDataCommand (ConnectionStringsRegistry connectionStringsRegistry) {
            if (connectionStringsRegistry == null) throw new ArgumentNullException (nameof (connectionStringsRegistry));

            _connectionString = connectionStringsRegistry.GetSqlServer ();
        }

        public bool Execute (SensorData data) {
            if (data is null) throw new ArgumentNullException (nameof (data));

            var ps = new DynamicParameters (data);

            ps.Add ("@Payload", JsonSerializer.Serialize (data, MyDefaults.JsonSerializerOptions));

            using IDbConnection connection = new SqlConnection (_connectionString);

            try {
                connection.Open ();
            } catch (SqlException sqlException) {
                Console.WriteLine ($"SqlServer is not yet ready. [{sqlException.Number}]");
                return false;
            }

            using var tran = connection.BeginTransaction ();

            try {
                connection.ExecuteScalar<int> (_statements[0], ps, commandType : CommandType.Text, transaction : tran);
                tran.Commit ();
                return true;
            } catch (Exception exception) {
                tran.Rollback ();
                Console.WriteLine (exception);
                return false;
            }
        }
    }
}