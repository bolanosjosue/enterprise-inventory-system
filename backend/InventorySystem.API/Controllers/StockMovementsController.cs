using InventorySystem.Application.StockMovements.Commands.ProcessPurchase;
using InventorySystem.Application.StockMovements.Commands.ProcessSale;
using InventorySystem.Application.StockMovements.Commands.TransferStock;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StockMovementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockMovementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("purchase")]
    public async Task<IActionResult> ProcessPurchase([FromBody] ProcessPurchaseCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpPost("sale")]
    public async Task<IActionResult> ProcessSale([FromBody] ProcessSaleCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpPost("transfer")]
    public async Task<IActionResult> TransferStock([FromBody] TransferStockCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }
}