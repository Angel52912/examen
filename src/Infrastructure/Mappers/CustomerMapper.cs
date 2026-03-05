using UtmMarket.Core.Entities;
using UtmMarket.Infrastructure.Models.Data;

namespace UtmMarket.Infrastructure.Mappers;

public static class CustomerMapper
{
    public static Customer ToDomain(this ClienteEntity entity)
    {
        return new Customer(entity.ClienteID, entity.Nombre, entity.Email)
        {
            RegistrationDate = entity.FechaRegistro
        };
    }
}
