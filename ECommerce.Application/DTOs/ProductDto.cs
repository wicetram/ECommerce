namespace ECommerce.Application.DTOs;

public sealed record ProductDto(Guid Id, string Name, decimal Amount, string Currency);
