using ECommerce.Application.Orders.Commands;
using ECommerce.WebApi.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController(IMediator mediator) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> Create([FromBody] CreateOrderHttpRequest body, CancellationToken ct)
    {
        var cmd = new CreateOrderCommand(
            Items: [.. body.Items.Select(i => new OrderItemInput(i.Id, i.Quantity, i.UnitPrice))],
            Currency: body.Currency
        );

        var result = await mediator.Send(cmd, ct);
        if (!result.IsSuccess)
            return Problem(title: result.Error?.Code.ToString(), detail: result.Error?.Message);

        return CreatedAtAction(nameof(GetById), new { id = result.Value!.Id }, result.Value);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] string id) => Ok(new { Id = id });

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete([FromRoute] string id, CancellationToken ct)
    {
        var result = await mediator.Send(new CompleteOrderCommand(id), ct);
        if (!result.IsSuccess)
            return Problem(title: result.Error?.Code.ToString(), detail: result.Error?.Message);

        return Ok(result.Value);
    }
}