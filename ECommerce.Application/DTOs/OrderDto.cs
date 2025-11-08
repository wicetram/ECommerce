namespace ECommerce.Application.DTOs;

public sealed record OrderDto(Guid Id, string Status, DateTime CreatedAt, decimal TotalAmount, string Currency);