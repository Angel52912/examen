using System.Data;

namespace UtmMarket.Infrastructure.Data;

public interface IDbConnectionFactory
{
    IDbConnection CreateConnection();
}