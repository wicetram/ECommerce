namespace ECommerce.Application.Common;

public sealed record Error(int Code, string Message)
{
    public static readonly Error Validation = new(-1, "Doğrulama hatası.");
    public static readonly Error NotFound = new(-99, "Kayıt bulunamadı.");
    public static Error External(int code, string message) => new(code, message);
}