namespace ECommerce.WebApi.Models;

public sealed class CreateOrderHttpRequest
{
    public string Currency { get; set; } = "USD";
    public List<CreateOrderHttpItem> Items { get; set; } = [];
}

public sealed class CreateOrderHttpItem
{
    public string Id { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}