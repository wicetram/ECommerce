using ECommerce.Domain.Exceptions;

namespace ECommerce.Domain.ValueObjects;

public sealed class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    private Money(decimal amount, string currency)
    {
        if (amount < 0)
            throw new DomainException("Money amount can not be negative");

        Amount = amount;
        Currency = currency;
    }

    public static Money Create(decimal amount, string currency) => new(amount, currency);

    public override string ToString() => $"{Amount} {Currency}";
}
