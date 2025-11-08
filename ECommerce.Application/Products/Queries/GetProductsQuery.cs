using ECommerce.Application.Common;
using ECommerce.Application.DTOs;
using MediatR;

namespace ECommerce.Application.Products.Queries;

public sealed record GetProductsQuery() : IRequest<Result<IReadOnlyList<ProductDto>>>;
