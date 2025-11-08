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
        // 1) Domain sipariş oluştur
        var order = new Order(Guid.NewGuid());

        foreach (var it in request.Items)
        {
            var unit = Money.Create(it.UnitPrice, request.Currency);
            var item = new OrderItem(it.ProductId, it.Quantity, unit);
            order.AddItem(item);
        }

        // 2) Preorder çağrısı (rezerv)
        var total = order.Items.Sum(i => i.UnitPrice.Amount * i.Quantity);
        var orderRef = order.Id.ToString("N");

        var preResp = await balanceClient.PreorderAsync(
            new PreorderRequest(orderRef, total, request.Currency), ct);

        if (!string.Equals(preResp.Status, "ok", StringComparison.OrdinalIgnoreCase))
        {
            var err = Error.External(preResp.ErrorCode,
                                     preResp.ErrorMessage ?? "İşlem başarısız.");
            return Result<OrderDto>.Failure(err);
        }

        // 3) Persist et
        await orders.AddAsync(order, ct);
        await uow.SaveChangesAsync(ct);

        var dto = mapper.Map<OrderDto>(order);
        return Result<OrderDto>.Success(dto);
    }
}