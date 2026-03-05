namespace UtmMarket.Infrastructure.Models.Data;

public record ClienteEntity(
    int ClienteID,
    string Nombre,
    string Email,
    DateTime FechaRegistro
);
