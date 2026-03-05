using System.Collections.Generic;
using UtmMarket.Core.Entities; // For SaleStatus

namespace UtmMarket.Core.UseCases.Sales.Commands;

public record CreateSaleCommand(string Folio, SaleStatus Status, IEnumerable<CreateSaleDetailCommand> Details);
