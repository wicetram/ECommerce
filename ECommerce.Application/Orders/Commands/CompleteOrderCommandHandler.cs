using AutoMapper;
using ECommerce.Application.Abstractions;
using ECommerce.Application.BalanceApi;
using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Orders.Commands;

public sealed class CompleteOrderCommandHandler(IOrderRepository orders, IUnitOfWork uow, IBalanceManagementClient balanceClient, IMapper mapper) : IRequestHandler<CompleteOrderCommand, Result<OrderDto>>
{
    public async Task<Result<OrderDto>> Handle(CompleteOrderCommand request, CancellationToken ct)
    {
        var order = await orders.GetByIdAsync(request.OrderId, ct);
        if (order is null)
            return Result<OrderDto>.Failure(Error.NotFound);

        var total = order.Items.Sum(i => i.UnitPrice.Amount * i.Quantity);
        var currency = order.Items.First().UnitPrice.Currency;
        var orderRef = order.Id.ToString("N");

        var complete = await balanceClient.CompleteAsync(
            new CompleteRequest(orderRef, total, currency), ct);

        if (!string.Equals(complete.Status, "ok", StringComparison.OrdinalIgnoreCase))
        {
            var err = Error.External(complete.ErrorCode,
                                     complete.ErrorMessage ?? "Tamamlama başarısız.");
            return Result<OrderDto>.Failure(err);
        }

        order.Complete();
        await orders.UpdateAsync(order, ct);
        await uow.SaveChangesAsync(ct);

        var dto = mapper.Map<OrderDto>(order);
        return Result<OrderDto>.Success(dto);
    }
}