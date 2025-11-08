using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Orders.Commands;

/// <summary>
/// Sipariş oluşturur ve Balance API üzerinden "preorder" (rezerv) çağrısı yapar.
/// </summary>
public sealed record CreateOrderCommand(IReadOnlyList<OrderItemInput> Items, string Currency) : IRequest<Result<OrderDto>>;

public sealed record OrderItemInput(Guid ProductId, int Quantity, decimal UnitPrice);