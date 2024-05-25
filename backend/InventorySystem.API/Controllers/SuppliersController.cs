using InventorySystem.Application.Suppliers.Commands.CreateSupplier;
using InventorySystem.Application.Suppliers.Queries.GetSupplierById;
using InventorySystem.Application.Suppliers.Queries.GetSuppliers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SuppliersController : ControllerBase
{
    private readonly IMediator _mediator;

    public SuppliersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetSuppliers()
    {
        var result = await _mediator.Send(new GetSuppliersQuery());

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetSupplierById(Guid id)
    {
        var result = await _mediator.Send(new GetSupplierByIdQuery(id));

        if (result.IsFailure)
            return NotFound(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> CreateSupplier([FromBody] CreateSupplierCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(nameof(GetSupplierById), new { id = result.Value.Id }, result.Value);
    }
}