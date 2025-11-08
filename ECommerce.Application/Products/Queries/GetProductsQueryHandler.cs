using ECommerce.Application.Abstractions;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Products.Queries;

public sealed class GetProductsQueryHandler(IBalanceManagementClient balanceClient) : IRequestHandler<GetProductsQuery, Result<IReadOnlyList<ProductDto>>>
{
    public async Task<Result<IReadOnlyList<ProductDto>>> Handle(GetProductsQuery request, CancellationToken ct)
    {
        // Dış servisten ürünleri al
        var resp = await balanceClient.GetProductsAsync(ct);

        var result = resp.Products
            .Select(p => new ProductDto(Guid.Parse(p.Id), p.Name, p.Price, p.Currency))
            .ToList()
            .AsReadOnly();

        return Result<IReadOnlyList<ProductDto>>.Success(result);
    }
}