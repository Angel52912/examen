USE [Test_utm_AGRZ];
GO

-- 1. Create table [dbo].[Cliente] if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cliente]') AND type in (N'U'))
BEGIN
    PRINT 'Creating table [dbo].[Cliente]...';
    CREATE TABLE [dbo].[Cliente] (
        ClienteID       INT IDENTITY(1,1)   PRIMARY KEY,
        Nombre          NVARCHAR(100)       NOT NULL,
        Email           NVARCHAR(100)       NOT NULL,
        FechaRegistro   DATETIME            NOT NULL DEFAULT GETDATE()
    );
    PRINT 'Table [dbo].[Cliente] created successfully.';
END
GO

-- 2. Add ClienteID column to [dbo].[Venta] table if it doesn't exist
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Venta]') AND name = 'ClienteID')
BEGIN
    PRINT 'Adding ClienteID column to [dbo].[Venta] table...';
    ALTER TABLE [dbo].[Venta]
    ADD ClienteID INT NULL; -- Allowing NULL for existing records if any

    ALTER TABLE [dbo].[Venta]
    ADD CONSTRAINT FK_Venta_Cliente FOREIGN KEY (ClienteID) REFERENCES [dbo].[Cliente](ClienteID);
    PRINT 'Column ClienteID added successfully.';
END
GO
