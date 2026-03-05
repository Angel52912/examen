using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices; // For [EnumeratorCancellation]
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Filters;
using UtmMarket.Core.Repositories;
using UtmMarket.Infrastructure.Data; // For IDbConnectionFactory
using UtmMarket.Infrastructure.Models.Data; // For VentaEntity, DetalleVentaEntity

namespace UtmMarket.Infrastructure.Repositories;

public class SaleRepositoryImpl(IDbConnectionFactory connectionFactory) : ISaleRepository
{
    private async IAsyncEnumerable<Sale> ExecuteQueryAndMapSales(
        SqlCommand command,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = (SqlConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        command.Connection = connection;

        using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        Dictionary<int, VentaEntity> sales = new();
        Dictionary<int, string?> customerNames = new();
        Dictionary<int, List<(DetalleVentaEntity detail, string productName, string sku)>> saleDetails = new();

        while (await reader.ReadAsync(cancellationToken))
        {
            int ventaId = reader.GetInt32(reader.GetOrdinal("VentaID"));

            if (!sales.ContainsKey(ventaId))
            {
                sales.Add(ventaId, new VentaEntity(
                    ventaId: ventaId,
                    folio: reader.GetString(reader.GetOrdinal("Folio")),
                    fechaVenta: reader.GetDateTime(reader.GetOrdinal("FechaVenta")),
                    totalArticulos: reader.GetInt32(reader.GetOrdinal("TotalArticulos")),
                    totalVenta: reader.GetDecimal(reader.GetOrdinal("TotalVenta")),
                    estatus: reader.GetByte(reader.GetOrdinal("Estatus")),
                    clienteId: reader.IsDBNull(reader.GetOrdinal("ClienteID")) ? null : reader.GetInt32(reader.GetOrdinal("ClienteID"))
                ));
                customerNames.Add(ventaId, reader.IsDBNull(reader.GetOrdinal("ClienteNombre")) ? null : reader.GetString(reader.GetOrdinal("ClienteNombre")));
                saleDetails.Add(ventaId, new List<(DetalleVentaEntity, string, string)>());
            }

            if (!reader.IsDBNull(reader.GetOrdinal("DetalleID")))
            {
                var detail = new DetalleVentaEntity(
                    detalleId: reader.GetInt32(reader.GetOrdinal("DetalleID")),
                    ventaId: ventaId,
                    productoId: reader.GetInt32(reader.GetOrdinal("ProductoID")),
                    precioUnitario: reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                    cantidad: reader.GetInt32(reader.GetOrdinal("Cantidad")),
                    totalDetalle: reader.GetDecimal(reader.GetOrdinal("TotalDetalle"))
                );
                string productName = reader.GetString(reader.GetOrdinal("ProductoNombre"));
                string sku = reader.GetString(reader.GetOrdinal("SKU"));
                saleDetails[ventaId].Add((detail, productName, sku));
            }
        }

        foreach (var ventaEntry in sales)
        {
            VentaEntity venta = ventaEntry.Value;
            var details = saleDetails.GetValueOrDefault(venta.VentaID, new List<(DetalleVentaEntity, string, string)>());
            string? customerName = customerNames.GetValueOrDefault(venta.VentaID);
            
            var sale = new Sale(venta.VentaID, venta.Folio, (SaleStatus)venta.Estatus, venta.ClienteID, customerName)
            {
                SaleDate = venta.FechaVenta
            };
            foreach(var d in details) {
                var p = new Product(d.detail.ProductoId, d.productName, d.sku, null, d.detail.PrecioUnitario, 0);
                sale.AddDetail(new SaleDetail(d.detail.DetalleID, p, d.detail.PrecioUnitario, d.detail.Cantidad));
            }
            yield return sale;
        }
    }

    public async IAsyncEnumerable<Sale> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        string query = @"
            SELECT
                v.VentaID, v.Folio, v.FechaVenta, v.TotalArticulos, v.TotalVenta, v.Estatus, v.ClienteID,
                c.Nombre AS ClienteNombre,
                dv.DetalleID, dv.ProductoID, dv.PrecioUnitario, dv.Cantidad, dv.TotalDetalle,
                p.Nombre AS ProductoNombre, p.SKU
            FROM Venta v
            LEFT JOIN Cliente c ON v.ClienteID = c.ClienteID
            LEFT JOIN DetalleVenta dv ON v.VentaID = dv.VentaID
            LEFT JOIN Producto p ON dv.ProductoID = p.ProductoID";

        SqlCommand command = new(query);
        await foreach (var sale in ExecuteQueryAndMapSales(command, cancellationToken))
        {
            yield return sale;
        }
    }

    public async Task<Sale?> GetByIdAsync(int saleId, CancellationToken cancellationToken = default)
    {
        string query = @"
            SELECT
                v.VentaID, v.Folio, v.FechaVenta, v.TotalArticulos, v.TotalVenta, v.Estatus, v.ClienteID,
                c.Nombre AS ClienteNombre,
                dv.DetalleID, dv.ProductoID, dv.PrecioUnitario, dv.Cantidad, dv.TotalDetalle,
                p.Nombre AS ProductoNombre, p.SKU
            FROM Venta v
            LEFT JOIN Cliente c ON v.ClienteID = c.ClienteID
            LEFT JOIN DetalleVenta dv ON v.VentaID = dv.VentaID
            LEFT JOIN Producto p ON dv.ProductoID = p.ProductoID
            WHERE v.VentaID = @SaleID";

        SqlCommand command = new(query);
        command.Parameters.AddWithValue("@SaleID", saleId);

        Sale? result = null;
        await foreach (var sale in ExecuteQueryAndMapSales(command, cancellationToken))
        {
            result = sale;
        }
        return result;
    }

    public async IAsyncEnumerable<Sale> FindAsync(SaleFilter filter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        List<string> conditions = new();
        List<SqlParameter> parameters = new();

        if (filter.SaleId.HasValue)
        {
            conditions.Add("v.VentaID = @SaleId");
            parameters.Add(new SqlParameter("@SaleId", filter.SaleId.Value));
        }
        if (filter.Folio is not null)
        {
            conditions.Add("v.Folio LIKE @Folio");
            parameters.Add(new SqlParameter("@Folio", $"%{filter.Folio}%"));
        }
        if (filter.StartDate.HasValue)
        {
            conditions.Add("v.FechaVenta >= @StartDate");
            parameters.Add(new SqlParameter("@StartDate", filter.StartDate.Value));
        }
        if (filter.EndDate.HasValue)
        {
            conditions.Add("v.FechaVenta <= @EndDate");
            parameters.Add(new SqlParameter("@EndDate", filter.EndDate.Value));
        }
        if (filter.Status.HasValue)
        {
            conditions.Add("v.Estatus = @Status");
            parameters.Add(new SqlParameter("@Status", (byte)filter.Status.Value));
        }

        string whereClause = conditions.Count > 0 ? " WHERE " + string.Join(" AND ", conditions) : string.Empty;
        string query = $@"
            SELECT
                v.VentaID, v.Folio, v.FechaVenta, v.TotalArticulos, v.TotalVenta, v.Estatus, v.ClienteID,
                c.Nombre AS ClienteNombre,
                dv.DetalleID, dv.ProductoID, dv.PrecioUnitario, dv.Cantidad, dv.TotalDetalle,
                p.Nombre AS ProductoNombre, p.SKU
            FROM Venta v
            LEFT JOIN Cliente c ON v.ClienteID = c.ClienteID
            LEFT JOIN DetalleVenta dv ON v.VentaID = dv.VentaID
            LEFT JOIN Producto p ON dv.ProductoID = p.ProductoID
            {whereClause}";

        SqlCommand command = new(query);
        command.Parameters.AddRange(parameters.ToArray());

        await foreach (var sale in ExecuteQueryAndMapSales(command, cancellationToken))
        {
            yield return sale;
        }
    }

    public async Task<Sale> AddAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = (SqlConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        SqlTransaction transaction = connection.BeginTransaction();

        try
        {
            string insertVentaSql = @"
                INSERT INTO Venta (Folio, FechaVenta, TotalArticulos, TotalVenta, Estatus, ClienteID)
                VALUES (@Folio, @FechaVenta, @TotalArticulos, @TotalVenta, @Estatus, @ClienteID);
                SELECT SCOPE_IDENTITY();";

            SqlCommand ventaCommand = new(insertVentaSql, connection, transaction);
            ventaCommand.Parameters.AddWithValue("@Folio", sale.Folio);
            ventaCommand.Parameters.AddWithValue("@FechaVenta", sale.SaleDate);
            ventaCommand.Parameters.AddWithValue("@TotalArticulos", sale.TotalItems);
            ventaCommand.Parameters.AddWithValue("@TotalVenta", sale.TotalSale);
            ventaCommand.Parameters.AddWithValue("@Estatus", (byte)sale.Status);
            ventaCommand.Parameters.AddWithValue("@ClienteID", (object?)sale.CustomerID ?? DBNull.Value);

            object? newVentaId = await ventaCommand.ExecuteScalarAsync(cancellationToken);
            if (newVentaId is null)
            {
                throw new InvalidOperationException("Failed to retrieve new Sale ID after insert.");
            }
            int ventaId = Convert.ToInt32(newVentaId);

            if (sale.SaleDetails.Any())
            {
                string insertDetalleSql = @"
                    INSERT INTO DetalleVenta (VentaID, ProductoID, PrecioUnitario, Cantidad, TotalDetalle)
                    VALUES (@VentaID, @ProductoID, @PrecioUnitario, @Cantidad, @TotalDetalle);";

                foreach (var detail in sale.SaleDetails)
                {
                    SqlCommand detalleCommand = new(insertDetalleSql, connection, transaction);
                    detalleCommand.Parameters.AddWithValue("@VentaID", ventaId);
                    detalleCommand.Parameters.AddWithValue("@ProductoID", detail.ProductID);
                    detalleCommand.Parameters.AddWithValue("@PrecioUnitario", detail.UnitPrice);
                    detalleCommand.Parameters.AddWithValue("@Cantidad", detail.Quantity);
                    detalleCommand.Parameters.AddWithValue("@TotalDetalle", detail.TotalDetalle);
                    await detalleCommand.ExecuteNonQueryAsync(cancellationToken);
                }
            }

            transaction.Commit();

            Sale newSale = new(ventaId, sale.Folio, sale.Status, sale.CustomerID, sale.CustomerName)
            {
                SaleDate = sale.SaleDate
            };
            foreach (var detail in sale.SaleDetails)
            {
                var dummyProduct = new Product(detail.ProductID, "Producto #" + detail.ProductID, "SKU", null, detail.UnitPrice, 0);
                newSale.AddDetail(new SaleDetail(detail.DetalleID, dummyProduct, detail.UnitPrice, detail.Quantity));
            }
            return newSale;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    public async Task UpdateAsync(Sale sale, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = (SqlConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        SqlTransaction transaction = connection.BeginTransaction();

        try
        {
            string updateVentaSql = @"
                UPDATE Venta
                SET Folio = @Folio, FechaVenta = @FechaVenta, TotalArticulos = @TotalArticulos, TotalVenta = @TotalVenta, Estatus = @Estatus
                WHERE VentaID = @VentaID;";

            SqlCommand ventaCommand = new(updateVentaSql, connection, transaction);
            ventaCommand.Parameters.AddWithValue("@Folio", sale.Folio);
            ventaCommand.Parameters.AddWithValue("@FechaVenta", sale.SaleDate);
            ventaCommand.Parameters.AddWithValue("@TotalArticulos", sale.TotalItems);
            ventaCommand.Parameters.AddWithValue("@TotalVenta", sale.TotalSale);
            ventaCommand.Parameters.AddWithValue("@Estatus", (byte)sale.Status);
            ventaCommand.Parameters.AddWithValue("@VentaID", sale.SaleID);

            int rowsAffected = await ventaCommand.ExecuteNonQueryAsync(cancellationToken);
            if (rowsAffected == 0)
            {
                transaction.Rollback();
                return;
            }

            SqlCommand deleteDetailsCommand = new("DELETE FROM DetalleVenta WHERE VentaID = @VentaID", connection, transaction);
            deleteDetailsCommand.Parameters.AddWithValue("@VentaID", sale.SaleID);
            await deleteDetailsCommand.ExecuteNonQueryAsync(cancellationToken);

            if (sale.SaleDetails.Any())
            {
                string insertDetalleSql = @"
                    INSERT INTO DetalleVenta (VentaID, ProductoID, PrecioUnitario, Cantidad, TotalDetalle)
                    VALUES (@VentaID, @ProductoID, @PrecioUnitario, @Cantidad, @TotalDetalle);";

                foreach (var detail in sale.SaleDetails)
                {
                    SqlCommand detalleCommand = new(insertDetalleSql, connection, transaction);
                    detalleCommand.Parameters.AddWithValue("@VentaID", sale.SaleID);
                    detalleCommand.Parameters.AddWithValue("@ProductoID", detail.ProductID);
                    detalleCommand.Parameters.AddWithValue("@PrecioUnitario", detail.UnitPrice);
                    detalleCommand.Parameters.AddWithValue("@Cantidad", detail.Quantity);
                    detalleCommand.Parameters.AddWithValue("@TotalDetalle", detail.TotalDetalle);
                    await detalleCommand.ExecuteNonQueryAsync(cancellationToken);
                }
            }

            transaction.Commit();
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }
}
