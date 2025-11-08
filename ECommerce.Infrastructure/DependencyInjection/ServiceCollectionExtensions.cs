using ECommerce.Application.Abstractions;
using ECommerce.Infrastructure.Config;
using ECommerce.Infrastructure.Http;
using ECommerce.Infrastructure.Persistence;
using ECommerce.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Contrib.WaitAndRetry;
using System.Net;
using System.Net.Http.Headers;

namespace ECommerce.Infrastructure.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        // Options
        var opt = new BalanceApiOptions();
        config.GetSection(BalanceApiOptions.SectionName).Bind(opt);

        services.AddSingleton(opt);

        // EF Core InMemory
        services.AddDbContext<AppDbContext>(o => o.UseInMemoryDatabase("ECommercePaymentDb"));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();

        // Polly policies
        var sleepDurations = Backoff.DecorrelatedJitterBackoffV2(
            medianFirstRetryDelay: TimeSpan.FromMilliseconds(200),
            retryCount: opt.RetryCount);

        var retryPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(r => (int)r.StatusCode >= 500 || r.StatusCode == HttpStatusCode.RequestTimeout)
            .WaitAndRetryAsync(sleepDurations);

        var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(opt.TimeoutSeconds));

        var breakerPolicy = Policy<HttpResponseMessage>
            .Handle<HttpRequestException>()
            .OrResult(r => (int)r.StatusCode >= 500 || r.StatusCode == HttpStatusCode.RequestTimeout)
            .CircuitBreakerAsync(
                handledEventsAllowedBeforeBreaking: opt.BreakerFailures,
                durationOfBreak: TimeSpan.FromSeconds(opt.BreakerDurationSeconds));

        // HttpClient
        services.AddHttpClient<IBalanceManagementClient, BalanceManagementClient>(client =>
        {
            client.BaseAddress = new Uri(opt.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(opt.TimeoutSeconds + 2);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        })
        .AddPolicyHandler(retryPolicy)
        .AddPolicyHandler(timeoutPolicy)
        .AddPolicyHandler(breakerPolicy);

        return services;
    }
}