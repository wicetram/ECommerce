using ECommerce.Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[ApiController]
[Route("api/products")]
public sealed class ProductsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken ct)
    {
        var result = await mediator.Send(new GetProductsQuery(), ct);
        if (!result.IsSuccess)
            return Problem(title: result.Error?.Code.ToString(), detail: result.Error?.Message);

        return Ok(result.Value);
    }
}