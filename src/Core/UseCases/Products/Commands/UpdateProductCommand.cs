namespace UtmMarket.Core.UseCases.Products.Commands;

public record UpdateProductCommand(int ProductID, string Name, string SKU, string? Brand, decimal Price, int Stock);
