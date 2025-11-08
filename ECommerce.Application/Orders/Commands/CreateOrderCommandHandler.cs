using AutoMapper;
using ECommerce.Application.Abstractions;
using ECommerce.Application.BalanceApi;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using ECommerce.Domain.Entities;
using ECommerce.Domain.ValueObjects;
using MediatR;

namespace ECommerce.Application.Orders.Commands;

public sealed class CreateOrderCommandHandler(IOrderRepository orders, IUnitOfWork uow, IBalanceManagementClient balanceClient, IMapper mapper) : IRequestHandler<CreateOrderCommand, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(CreateOrderCommand request, CancellationToken ct)
    {
        var order = new Order(Guid.NewGuid().ToString("N"));

        foreach (var it in request.Items)
        {
            var unit = Money.Create(it.UnitPrice, request.Currency);
            var item = new OrderItem(it.ProductId, it.Quantity, unit);
            order.AddItem(item);
        }

        var total = order.Items.Sum(i => i.UnitPrice.Amount * i.Quantity);
        var orderRef = order.Id;

        var preResp = await balanceClient.PreorderAsync(
            new PreorderRequest(OrderId: orderRef, Amount: total), ct);

        if (!preResp.Success)
        {
            var err = Error.External(-1, preResp.Message ?? "İşlem başarısız.");
            return Result<OrderDto>.Failure(err);
        }

        await orders.AddAsync(order, ct);
        await uow.SaveChangesAsync(ct);

        var dto = mapper.Map<OrderDto>(order);
        return Result<OrderDto>.Success(dto);
    }
}