using FluentValidation;

namespace ECommerce.Application.Orders.Commands;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Currency)
            .NotEmpty().WithMessage("Para birimi zorunludur.")
            .MaximumLength(8);

        RuleFor(x => x.Items)
            .NotEmpty().WithMessage("En az bir satır girilmelidir.");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId).NotEmpty();
                item.RuleFor(i => i.Quantity).GreaterThan(0);
                item.RuleFor(i => i.UnitPrice).GreaterThan(0);
            });
    }
}