using ECommerce.Application.BalanceApi;

namespace ECommerce.Application.Abstractions;

/// <summary>
/// Balance Management servisine karşı port.
/// Infrastructure katmanında HttpClient ile implemente edilir.
/// </summary>
public interface IBalanceManagementClient
{
    Task<GetProductsResponse> GetProductsAsync(CancellationToken ct);
    Task<PreorderResponse> PreorderAsync(PreorderRequest request, CancellationToken ct);
    Task<CompleteResponse> CompleteAsync(CompleteRequest request, CancellationToken ct);
}