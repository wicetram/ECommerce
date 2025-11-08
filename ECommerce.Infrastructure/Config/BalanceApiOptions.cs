namespace ECommerce.Infrastructure.Config;

public sealed class BalanceApiOptions
{
    public const string SectionName = "BalanceApi";
    public string BaseUrl { get; set; } = default!;
    public int TimeoutSeconds { get; set; } = 3;
    public int RetryCount { get; set; } = 2;
    public int BreakerFailures { get; set; } = 3;
    public int BreakerDurationSeconds { get; set; } = 30;
}
