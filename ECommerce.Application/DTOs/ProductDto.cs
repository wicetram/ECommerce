namespace ECommerce.Application.DTOs;

public sealed record ProductDto(string Id, string Name, string Description, decimal Price, string Currency, string Category, int Stock);
