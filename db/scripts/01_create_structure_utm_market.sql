SET NOCOUNT ON;
SET XACT_ABORT ON;
-- Consider changing '[data_base_name]' to your actual database name if it's not set in the connection.
USE [Test_utm_AGRZ];
GO

PRINT 'Starting database structure creation...';

--
-- Tabla Producto
--
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Producto]') AND type in (N'U'))
BEGIN
    PRINT 'Creating table [dbo].[Producto]...';
    CREATE TABLE [dbo].[Producto] (
        ProductoID      INT IDENTITY(1,1)   PRIMARY KEY,
        Nombre          NVARCHAR(100)       NOT NULL,
        SKU             VARCHAR(20)         NOT NULL UNIQUE,
        Marca           NVARCHAR(50)        NULL,
        Precio          DECIMAL(19,4)       NOT NULL,
        Stock           INT                 NOT NULL,
        CONSTRAINT CK_Producto_Precio CHECK (Precio >= 0),
        CONSTRAINT CK_Producto_Stock CHECK (Stock >= 0)
    );
    PRINT 'Table [dbo].[Producto] created successfully.';
END
ELSE
BEGIN
    PRINT 'Table [dbo].[Producto] already exists. Skipping creation.';
END
GO

--
-- Tabla Venta
--
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Venta]') AND type in (N'U'))
BEGIN
    PRINT 'Creating table [dbo].[Venta]...';
    CREATE TABLE [dbo].[Venta] (
        VentaID         INT IDENTITY(1,1)   PRIMARY KEY,
        Folio           VARCHAR(20)         NOT NULL UNIQUE,
        FechaVenta      DATETIME            NOT NULL DEFAULT GETDATE(),
        TotalArticulos  INT                 NOT NULL,
        TotalVenta      DECIMAL(19,4)       NOT NULL,
        Estatus         TINYINT             NOT NULL,
        -- Estatus: 1 = Pendiente, 2 = Completada, 3 = Cancelada
        CONSTRAINT CK_Venta_Estatus CHECK (Estatus IN (1, 2, 3))
    );
    PRINT 'Table [dbo].[Venta] created successfully.';
END
ELSE
BEGIN
    PRINT 'Table [dbo].[Venta] already exists. Skipping creation.';
END
GO

--
-- Tabla Detalle de Venta
--
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DetalleVenta]') AND type in (N'U'))
BEGIN
    PRINT 'Creating table [dbo].[DetalleVenta]...';
    CREATE TABLE [dbo].[DetalleVenta] (
        DetalleID       INT IDENTITY(1,1)   PRIMARY KEY,
        VentaID         INT                 NOT NULL,
        ProductoID      INT                 NOT NULL,
        PrecioUnitario  DECIMAL(19,4)       NOT NULL,
        Cantidad        INT                 NOT NULL,
        TotalDetalle    DECIMAL(19,4)       NOT NULL,
        CONSTRAINT FK_DetalleVenta_Venta FOREIGN KEY (VentaID) REFERENCES [dbo].[Venta](VentaID),
        CONSTRAINT FK_DetalleVenta_Producto FOREIGN KEY (ProductoID) REFERENCES [dbo].[Producto](ProductoID),
        CONSTRAINT CK_DetalleVenta_PrecioUnitario CHECK (PrecioUnitario >= 0),
        CONSTRAINT CK_DetalleVenta_Cantidad CHECK (Cantidad >= 0),
        CONSTRAINT CK_DetalleVenta_TotalDetalle CHECK (TotalDetalle >= 0)
    );
    PRINT 'Table [dbo].[DetalleVenta] created successfully.';
END
ELSE
BEGIN
    PRINT 'Table [dbo].[DetalleVenta] already exists. Skipping creation.';
END
GO

PRINT 'Database structure creation complete.';
