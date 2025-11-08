namespace ECommerce.Application.Common;

public sealed record Error(string Code, string Message)
{
    public static readonly Error Validation = new("validation_error", "Doğrulama hatası.");
    public static readonly Error NotFound = new("not_found", "Kayıt bulunamadı.");
    public static Error External(string code, string message) => new(code, message);
}