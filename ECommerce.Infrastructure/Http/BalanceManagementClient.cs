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
        // GET /api/products
        using var resp = await http.GetAsync("/api/products", ct);
        await EnsureSuccess(resp, ct);

        var payload = await resp.Content.ReadFromJsonAsync<BalanceProduct[]>(JsonOpts, ct) ?? [];

        return new GetProductsResponse(payload);
    }

    public async Task<PreorderResponse> PreorderAsync(PreorderRequest request, CancellationToken ct)
    {
        // POST /api/balance/preorder
        using var resp = await http.PostAsJsonAsync("/api/balance/preorder", request, JsonOpts, ct);
        // Bu API 4xx/5xx dönse bile body’de anlamlı mesaj olabilir; önce body’yi dene
        if (!resp.IsSuccessStatusCode && resp.StatusCode != HttpStatusCode.BadRequest)
            await EnsureSuccess(resp, ct);

        var payload = await resp.Content.ReadFromJsonAsync<PreorderResponse>(JsonOpts, ct)
                      ?? new PreorderResponse("error", -1, "Empty response");

        return payload;
    }

    public async Task<CompleteResponse> CompleteAsync(CompleteRequest request, CancellationToken ct)
    {
        // POST /api/balance/complete
        using var resp = await http.PostAsJsonAsync("/api/balance/complete", request, JsonOpts, ct);
        if (!resp.IsSuccessStatusCode && resp.StatusCode != HttpStatusCode.BadRequest)
            await EnsureSuccess(resp, ct);

        var payload = await resp.Content.ReadFromJsonAsync<CompleteResponse>(JsonOpts, ct)
                      ?? new CompleteResponse("error", -1, "Empty response");

        return payload;
    }

    private static async Task EnsureSuccess(HttpResponseMessage resp, CancellationToken ct)
    {
        if (resp.IsSuccessStatusCode) return;

        var body = resp.Content is null ? string.Empty : await resp.Content.ReadAsStringAsync(ct);
        throw new HttpRequestException(
            $"Balance API error: {(int)resp.StatusCode} {resp.ReasonPhrase}. Body: {body}");
    }
}