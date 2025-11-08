using ECommerce.Application.Abstractions;
using ECommerce.Application.BalanceApi;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ECommerce.Infrastructure.Http;

public sealed class BalanceManagementClient(HttpClient http) : IBalanceManagementClient
{
    static readonly JsonSerializerOptions JsonOpts = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    public async Task<GetProductsResponse> GetProductsAsync(CancellationToken ct)
    {
        using var resp = await http.GetAsync("/api/products", ct);
        await EnsureSuccess(resp, ct);

        var payload = await resp.Content.ReadFromJsonAsync<GetProductsResponse>(JsonOpts, ct)
                      ?? new GetProductsResponse(false, []);

        return payload;
    }

    public async Task<PreorderResponse> PreorderAsync(PreorderRequest request, CancellationToken ct)
    {
        using var resp = await http.PostAsJsonAsync("/api/balance/preorder", request, JsonOpts, ct);

        // 400'de de anlamlı gövde geliyor olabilir
        if (!resp.IsSuccessStatusCode && resp.StatusCode != HttpStatusCode.BadRequest)
            await EnsureSuccess(resp, ct);

        var payload = await resp.Content.ReadFromJsonAsync<PreorderResponse>(JsonOpts, ct)
                      ?? new PreorderResponse(false, "Empty response", null);

        return payload;
    }

    public async Task<CompleteResponse> CompleteAsync(CompleteRequest request, CancellationToken ct)
    {
        using var resp = await http.PostAsJsonAsync("/api/balance/complete", request, JsonOpts, ct);

        if (!resp.IsSuccessStatusCode && resp.StatusCode != HttpStatusCode.BadRequest)
            await EnsureSuccess(resp, ct);

        var payload = await resp.Content.ReadFromJsonAsync<CompleteResponse>(JsonOpts, ct)
                      ?? new CompleteResponse(false, "Empty response", null);

        return payload;
    }

    private static async Task EnsureSuccess(HttpResponseMessage resp, CancellationToken ct)
    {
        if (resp.IsSuccessStatusCode) return;
        var body = resp.Content is null ? "" : await resp.Content.ReadAsStringAsync(ct);
        throw new HttpRequestException($"Balance API error: {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {body}");
    }
}