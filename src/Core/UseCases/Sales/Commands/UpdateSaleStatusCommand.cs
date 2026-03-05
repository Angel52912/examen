using UtmMarket.Core.Entities; // For SaleStatus

namespace UtmMarket.Core.UseCases.Sales.Commands;

public record UpdateSaleStatusCommand(int SaleID, SaleStatus NewStatus);
