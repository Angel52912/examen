namespace UtmMarket.Core.UseCases.Products.Commands;

public record UpdateProductStockCommand(int ProductID, int NewStock);
