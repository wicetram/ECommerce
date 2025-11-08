namespace ECommerce.Application.BalanceApi;

// Get Products
public sealed record GetProductsResponse(IReadOnlyList<BalanceProduct> Products);
public sealed record BalanceProduct(string Id, string Name, decimal Price, string Currency);

// Preorder (reserve funds)
public sealed record PreorderRequest(string OrderReference, decimal TotalAmount, string Currency);
public sealed record PreorderResponse(string Status, int ErrorCode, string? ErrorMessage);

// Complete
public sealed record CompleteRequest(string OrderReference, decimal TotalAmount, string Currency);
public sealed record CompleteResponse(string Status, int ErrorCode, string? ErrorMessage);