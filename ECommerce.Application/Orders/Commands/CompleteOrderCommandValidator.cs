using FluentValidation;

namespace ECommerce.Application.Orders.Commands;

public sealed class CompleteOrderCommandValidator : AbstractValidator<CompleteOrderCommand>
{
    public CompleteOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
    }
}