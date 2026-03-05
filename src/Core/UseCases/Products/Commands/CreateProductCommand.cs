namespace UtmMarket.Core.UseCases.Products.Commands;

public record CreateProductCommand(string Name, string SKU, string? Brand, decimal Price, int Stock);
