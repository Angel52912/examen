using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Infrastructure.Data;
using UtmMarket.Infrastructure.Models.Data;
using UtmMarket.Infrastructure.Mappers;

namespace UtmMarket.Infrastructure.Repositories;

public class CustomerRepositoryImpl(IDbConnectionFactory connectionFactory) : ICustomerRepository
{
    private static ClienteEntity MapFromReader(SqlDataReader reader) => new(
        ClienteID: reader.GetInt32(reader.GetOrdinal("ClienteID")),
        Nombre: reader.GetString(reader.GetOrdinal("Nombre")),
        Email: reader.GetString(reader.GetOrdinal("Email")),
        FechaRegistro: reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
    );

    public async IAsyncEnumerable<Customer> GetAllAsync([EnumeratorCancellation] CancellationToken ct = default)
    {
        using var conn = (SqlConnection)connectionFactory.CreateConnection();
        await conn.OpenAsync(ct);
        using var cmd = new SqlCommand("SELECT ClienteID, Nombre, Email, FechaRegistro FROM Cliente", conn);
        using var reader = await cmd.ExecuteReaderAsync(ct);

        while (await reader.ReadAsync(ct))
        {
            yield return MapFromReader(reader).ToDomain();
        }
    }

    public async Task<Customer?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        using var conn = (SqlConnection)connectionFactory.CreateConnection();
        await conn.OpenAsync(ct);
        using var cmd = new SqlCommand("SELECT ClienteID, Nombre, Email, FechaRegistro FROM Cliente WHERE ClienteID = @Id", conn);
        cmd.Parameters.AddWithValue("@Id", id);
        
        using var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SingleRow, ct);
        if (await reader.ReadAsync(ct))
        {
            return MapFromReader(reader).ToDomain();
        }
        return null;
    }

    public async Task<Customer> AddAsync(Customer customer, CancellationToken ct = default)
    {
        using var conn = (SqlConnection)connectionFactory.CreateConnection();
        await conn.OpenAsync(ct);
        const string sql = @"INSERT INTO Cliente (Nombre, Email, FechaRegistro) 
                             VALUES (@Nombre, @Email, @Fecha); 
                             SELECT SCOPE_IDENTITY();";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nombre", customer.Name);
        cmd.Parameters.AddWithValue("@Email", customer.Email);
        cmd.Parameters.AddWithValue("@Fecha", customer.RegistrationDate);

        var id = await cmd.ExecuteScalarAsync(ct);
        return new Customer(Convert.ToInt32(id), customer.Name, customer.Email) 
        { 
            RegistrationDate = customer.RegistrationDate 
        };
    }

    public async Task<bool> UpdateAsync(Customer customer, CancellationToken ct = default)
    {
        using var conn = (SqlConnection)connectionFactory.CreateConnection();
        await conn.OpenAsync(ct);
        const string sql = "UPDATE Cliente SET Nombre = @Nombre, Email = @Email WHERE ClienteID = @Id";

        using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nombre", customer.Name);
        cmd.Parameters.AddWithValue("@Email", customer.Email);
        cmd.Parameters.AddWithValue("@Id", customer.CustomerID);

        return (await cmd.ExecuteNonQueryAsync(ct)) > 0;
    }
}
