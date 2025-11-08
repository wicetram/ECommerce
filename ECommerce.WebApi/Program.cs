using ECommerce.Application.Mapping;
using ECommerce.Application.Orders.Commands;
using ECommerce.Infrastructure.DependencyInjection;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Controllers + FluentValidation (automatic model validation)
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateOrderCommandValidator>();

// MediatR & AutoMapper
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateOrderCommand>());
builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<MappingProfile>(); }, typeof(MappingProfile).Assembly);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Infrastructure (EF InMemory + HttpClient+Polly)
builder.Services.AddInfrastructure(builder.Configuration);

// HealthChecks (self + future deps)
builder.Services.AddHealthChecks();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

// Global error handler (very simple)
app.Use(async (ctx, next) =>
{
    try { await next(); }
    catch (Exception ex)
    {
        ctx.Response.StatusCode = StatusCodes.Status500InternalServerError;
        ctx.Response.ContentType = "application/json";
        await ctx.Response.WriteAsJsonAsync(new
        {
            error = -99,
            message = ex.Message
        });
    }
});

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();