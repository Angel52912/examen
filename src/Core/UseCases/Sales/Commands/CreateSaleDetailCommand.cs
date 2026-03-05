namespace UtmMarket.Core.UseCases.Sales.Commands;

public record CreateSaleDetailCommand(int ProductID, decimal UnitPrice, int Quantity);
