using InventorySystem.Application.Warehouses.Commands.CreateWarehouse;
using InventorySystem.Application.Warehouses.Queries.GetWarehouseById;
using InventorySystem.Application.Warehouses.Queries.GetWarehouses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WarehousesController : ControllerBase
{
    private readonly IMediator _mediator;

    public WarehousesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetWarehouses()
    {
        var result = await _mediator.Send(new GetWarehousesQuery());

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetWarehouseById(Guid id)
    {
        var result = await _mediator.Send(new GetWarehouseByIdQuery(id));

        if (result.IsFailure)
            return NotFound(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateWarehouse([FromBody] CreateWarehouseCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(nameof(GetWarehouseById), new { id = result.Value.Id }, result.Value);
    }
}