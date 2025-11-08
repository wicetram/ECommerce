namespace ECommerce.Application.DTOs;

public sealed record OrderItemDto(Guid ProductId, int Quantity, decimal UnitPrice, string Currency);