using ECommerce.Application.Abstractions;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Products.Queries;

public sealed class GetProductsQueryHandler(IBalanceManagementClient balanceClient) : IRequestHandler<GetProductsQuery, Result<IReadOnlyList<ProductDto>>>
{
    public async Task<Result<IReadOnlyList<ProductDto>>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        var resp = await balanceClient.GetProductsAsync(ct);
        if (!resp.Success || resp.Data is null)
            return Result<IReadOnlyList<ProductDto>>.Failure(Error.External(-1, "Ürünler getirilemedi."));

        var result = resp.Data
            .Select(p => new ProductDto(p.Id, p.Name, p.Description, p.Price, p.Currency, p.Category, p.Stock))
            .ToList()
            .AsReadOnly();

        return Result<IReadOnlyList<ProductDto>>.Success(result);
    }
}