using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UtmMarket.Core.Entities;
using UtmMarket.Core.Repositories;
using UtmMarket.Core.UseCases;
using UtmMarket.Core.UseCases.Sales.Commands;

namespace UtmMarket.Application;

/// <summary>
/// Implementación concreta del caso de uso para crear una venta.
/// </summary>
public sealed class CreateSaleUseCaseImpl(ISaleRepository saleRepository, IProductRepository productRepository) : ICreateSaleUseCase
{
    public async ValueTask<Sale> ExecuteAsync(CreateSaleCommand command, CancellationToken cancellationToken = default)
    {
        // Validación básica
        if (string.IsNullOrWhiteSpace(command.Folio))
        {
            throw new ArgumentException("El folio de la venta no puede estar vacío.", nameof(command.Folio));
        }

        if (!command.Details.Any())
        {
            throw new ArgumentException("La venta debe tener al menos un detalle.", nameof(command.Details));
        }

        var sale = new Sale(0, command.Folio, command.Status);

        foreach (var detailCommand in command.Details)
        {
            var product = await productRepository.GetByIdAsync(detailCommand.ProductID, cancellationToken);
            if (product == null)
            {
                throw new InvalidOperationException($"El producto con ID {detailCommand.ProductID} no existe.");
            }

            if (product.Stock < detailCommand.Quantity)
            {
                throw new InvalidOperationException($"Stock insuficiente para el producto {product.Name}. Disponible: {product.Stock}, Solicitado: {detailCommand.Quantity}");
            }

            var detail = new SaleDetail(
                detailId: 0,
                product: product,
                unitPrice: detailCommand.UnitPrice,
                quantity: detailCommand.Quantity
            );

            sale.AddDetail(detail);

            // Actualizar stock del producto (Reducción)
            await productRepository.UpdateStockAsync(product.ProductID, product.Stock - detailCommand.Quantity, cancellationToken);
        }

        return await saleRepository.AddAsync(sale, cancellationToken);
    }
}
