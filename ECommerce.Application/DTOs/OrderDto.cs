namespace ECommerce.Application.DTOs;

public sealed record OrderDto(string Id, string Status, DateTime CreatedAt, decimal TotalAmount, string Currency);