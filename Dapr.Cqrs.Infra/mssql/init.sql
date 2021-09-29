Use [master] 

GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'explorer') BEGIN
      CREATE DATABASE [explorer]
    END

GO

Use [explorer]

GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='SensorData' and xtype='U') BEGIN
        CREATE TABLE SensorData (
            Id uniqueidentifier PRIMARY KEY,
            Plant VARCHAR(5) NOT NULL ,
            Location NVARCHAR(50) NOT NULL ,
            Tag VARCHAR(25) NOT NULL ,
            Value DECIMAL(10,8) NOT NULL ,
            RecordedOn DATETIME2 NOT NULL, 
            CreatedOn DATETIME2 NOT NULL 
        )
    END

GO

IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Outbox' and xtype='U') BEGIN
        CREATE TABLE Outbox (
            EventId uniqueidentifier NOT NULL,
            StorageType INT NOT NULL,
            [Status] INT NOT NULL DEFAULT(0),
            Payload NVARCHAR(max) NOT NULL ,
            Retry int NOT NULL DEFAULT(0),
            CreatedOn DATETIME2 NOT NULL ,
            UpdatedOn DATETIME2 NULL ,
            CONSTRAINT Outbox_Pk PRIMARY KEY (StorageType, EventId)
        )
    END

GO

