using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices; // Required for [EnumeratorCancellation]
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Filters;
using UtmMarket.Core.Repositories;
using UtmMarket.Infrastructure.Data; // For IDbConnectionFactory
using UtmMarket.Infrastructure.Models.Data; // For ProductoEntity

namespace UtmMarket.Infrastructure.Repositories;

public class ProductRepositoryImpl(IDbConnectionFactory connectionFactory) : IProductRepository
{
    // Helper method to map from SqlDataReader to ProductoEntity
    private static ProductoEntity MapToProductoEntity(SqlDataReader reader)
    {
        return new ProductoEntity(
            productoId: reader.GetInt32(reader.GetOrdinal("ProductoID")),
            nombre: reader.GetString(reader.GetOrdinal("Nombre")),
            sku: reader.GetString(reader.GetOrdinal("SKU")),
            marca: reader.IsDBNull(reader.GetOrdinal("Marca")) ? null : reader.GetString(reader.GetOrdinal("Marca")),
            precio: reader.GetDecimal(reader.GetOrdinal("Precio")),
            stock: reader.GetInt32(reader.GetOrdinal("Stock"))
        );
    }

    public async IAsyncEnumerable<Product> GetAllAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = (SqlConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        using SqlCommand command = new("SELECT ProductoID, Nombre, SKU, Marca, Precio, Stock FROM Producto", connection);
        command.CommandType = CommandType.Text;
        using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            var entity = MapToProductoEntity(reader);
            yield return new Product(entity.ProductoID, entity.Nombre, entity.SKU, entity.Marca, entity.Precio, entity.Stock);
        }
    }

    public async Task<Product?> GetByIdAsync(int productId, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = (SqlConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);
        using SqlCommand command = new("SELECT ProductoID, Nombre, SKU, Marca, Precio, Stock FROM Producto WHERE ProductoID = @ProductoID", connection);
        command.CommandType = CommandType.Text;
        command.Parameters.AddWithValue("@ProductoID", productId);

        using SqlDataReader reader = await command.ExecuteReaderAsync(CommandBehavior.SingleRow, cancellationToken);
        if (await reader.ReadAsync(cancellationToken))
        {
            var entity = MapToProductoEntity(reader);
            return new Product(entity.ProductoID, entity.Nombre, entity.SKU, entity.Marca, entity.Precio, entity.Stock);
        }
        return null;
    }

    public async IAsyncEnumerable<Product> FindAsync(ProductFilter filter, [EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = (SqlConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        List<string> conditions = new();
        List<SqlParameter> parameters = new();

        if (filter.Name is not null)
        {
            conditions.Add("Nombre LIKE @Nombre");
            parameters.Add(new SqlParameter("@Nombre", $"%{filter.Name}%"));
        }
        if (filter.SKU is not null)
        {
            conditions.Add("SKU LIKE @SKU");
            parameters.Add(new SqlParameter("@SKU", $"%{filter.SKU}%"));
        }
        if (filter.Brand is not null)
        {
            conditions.Add("Marca LIKE @Marca");
            parameters.Add(new SqlParameter("@Marca", $"%{filter.Brand}%"));
        }
        if (filter.MinPrice.HasValue)
        {
            conditions.Add("Precio >= @MinPrice");
            parameters.Add(new SqlParameter("@MinPrice", filter.MinPrice.Value));
        }
        if (filter.MaxPrice.HasValue)
        {
            conditions.Add("Precio <= @MaxPrice");
            parameters.Add(new SqlParameter("@MaxPrice", filter.MaxPrice.Value));
        }
        if (filter.MinStock.HasValue)
        {
            conditions.Add("Stock >= @MinStock");
            parameters.Add(new SqlParameter("@MinStock", filter.MinStock.Value));
        }
        if (filter.MaxStock.HasValue)
        {
            conditions.Add("Stock <= @MaxStock");
            parameters.Add(new SqlParameter("@MaxStock", filter.MaxStock.Value));
        }

        string whereClause = conditions.Count > 0 ? " WHERE " + string.Join(" AND ", conditions) : string.Empty;
        string query = $"SELECT ProductoID, Nombre, SKU, Marca, Precio, Stock FROM Producto{whereClause}";

        using SqlCommand command = new(query, connection);
        command.CommandType = CommandType.Text;
        command.Parameters.AddRange(parameters.ToArray());

        using SqlDataReader reader = await command.ExecuteReaderAsync(cancellationToken);

        while (await reader.ReadAsync(cancellationToken))
        {
            var entity = MapToProductoEntity(reader);
            yield return new Product(entity.ProductoID, entity.Nombre, entity.SKU, entity.Marca, entity.Precio, entity.Stock);
        }
    }

    public async Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = (SqlConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        string insertSql = @"
            INSERT INTO Producto (Nombre, SKU, Marca, Precio, Stock)
            VALUES (@Nombre, @SKU, @Marca, @Precio, @Stock);
            SELECT SCOPE_IDENTITY();";

        using SqlCommand command = new(insertSql, connection);
        command.CommandType = CommandType.Text;

        command.Parameters.AddWithValue("@Nombre", product.Name);
        command.Parameters.AddWithValue("@SKU", product.SKU);
        command.Parameters.AddWithValue("@Marca", product.Brand ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Precio", product.Price);
        command.Parameters.AddWithValue("@Stock", product.Stock);

        object? newId = await command.ExecuteScalarAsync(cancellationToken);
        if (newId is decimal decimalId)
        {
            int productId = Convert.ToInt32(decimalId);
            return new Product(productId, product.Name, product.SKU, product.Brand, product.Price, product.Stock);
        }
        throw new InvalidOperationException("Failed to retrieve new product ID after insert.");
    }

    public async Task<bool> UpdateAsync(Product product, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = (SqlConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        string updateSql = @"
            UPDATE Producto
            SET Nombre = @Nombre, SKU = @SKU, Marca = @Marca, Precio = @Precio, Stock = @Stock
            WHERE ProductoID = @ProductoID;";

        using SqlCommand command = new(updateSql, connection);
        command.CommandType = CommandType.Text;

        command.Parameters.AddWithValue("@Nombre", product.Name);
        command.Parameters.AddWithValue("@SKU", product.SKU);
        command.Parameters.AddWithValue("@Marca", product.Brand ?? (object)DBNull.Value);
        command.Parameters.AddWithValue("@Precio", product.Price);
        command.Parameters.AddWithValue("@Stock", product.Stock);
        command.Parameters.AddWithValue("@ProductoID", product.ProductID);

        int rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
        return rowsAffected > 0;
    }

    public async Task<bool> UpdateStockAsync(int productId, int newStock, CancellationToken cancellationToken = default)
    {
        using SqlConnection connection = (SqlConnection)connectionFactory.CreateConnection();
        await connection.OpenAsync(cancellationToken);

        string updateSql = @"
            UPDATE Producto
            SET Stock = @NewStock
            WHERE ProductoID = @ProductoID;";

        using SqlCommand command = new(updateSql, connection);
        command.CommandType = CommandType.Text;

        command.Parameters.AddWithValue("@NewStock", newStock);
        command.Parameters.AddWithValue("@ProductoID", productId);

        int rowsAffected = await command.ExecuteNonQueryAsync(cancellationToken);
        return rowsAffected > 0;
    }
}
