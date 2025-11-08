namespace ECommerce.Application.BalanceApi;

// -------------------- Products --------------------
public sealed record GetProductsResponse(bool Success, IReadOnlyList<BalanceProduct> Data);
public sealed record BalanceProduct(string Id, string Name, string Description, decimal Price, string Currency, string Category, int Stock);

// -------------------- Preorder (reserve funds) --------------------
// Request: { "amount": 19.99, "orderId": "..." }
public sealed record PreorderRequest(string OrderId, decimal Amount);

// Response:
public sealed record PreorderResponse(bool Success, string Message, PreorderData? Data);
public sealed record PreorderData(PreOrderInfo PreOrder, UpdatedBalance UpdatedBalance);

public sealed record PreOrderInfo(string OrderId, decimal Amount, DateTime Timestamp, string Status);

public sealed record UpdatedBalance(string UserId, decimal TotalBalance, decimal AvailableBalance, decimal BlockedBalance, string Currency, DateTime LastUpdated);

// -------------------- Complete --------------------
// Request: { "orderId": "..." }
public sealed record CompleteRequest(string OrderId);

// Response:
public sealed record CompleteResponse(bool Success, string Message, CompleteData? Data);
public sealed record CompleteData(OrderInfo Order, UpdatedBalance UpdatedBalance);

public sealed record OrderInfo(string OrderId, decimal Amount, DateTime Timestamp, string Status, DateTime CompletedAt);