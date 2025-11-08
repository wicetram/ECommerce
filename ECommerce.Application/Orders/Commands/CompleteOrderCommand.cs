using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Orders.Commands;

/// <summary>
/// Preorder alınmış siparişi tamamlar ve Balance API "complete" çağrısı yapar.
/// </summary>
public sealed record CompleteOrderCommand(string OrderId) : IRequest<Result<OrderDto>>;