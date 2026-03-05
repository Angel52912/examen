using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using UtmMarket.Infrastructure.Configuration;

namespace UtmMarket.Infrastructure.Data;

public class SqlConnectionFactory(IOptions<DatabaseOptions> options) : IDbConnectionFactory
{
    private readonly string _connectionString = options.Value.DefaultConnection;

    public IDbConnection CreateConnection()
    {
        return new SqlConnection(_connectionString);
    }
}