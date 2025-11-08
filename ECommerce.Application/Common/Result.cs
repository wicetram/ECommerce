namespace ECommerce.Application.Common;

/// <summary>
/// Uygulama genelinde tutarlı dönüş gövdesi.
/// </summary>
public readonly record struct Result(bool IsSuccess, Error? Error)
{
    public static Result Success() => new(true, null);
    public static Result Failure(Error error) => new(false, error);
}

public readonly record struct Result<T>(bool IsSuccess, T? Value, Error? Error)
{
    public static Result<T> Success(T value) => new(true, value, null);
    public static Result<T> Failure(Error error) => new(false, default, error);
}