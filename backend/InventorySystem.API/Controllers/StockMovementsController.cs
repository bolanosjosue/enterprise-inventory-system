using InventorySystem.Application.StockMovements.Commands.ProcessPurchase;
using InventorySystem.Application.StockMovements.Commands.ProcessSale;
using InventorySystem.Application.StockMovements.Commands.TransferStock;
using InventorySystem.Application.StockMovements.Queries.GetMovementHistory;
using InventorySystem.Application.StockMovements.Queries.GetStockByWarehouse;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class StockMovementsController : ControllerBase
{
    private readonly IMediator _mediator;

    public StockMovementsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetMovementHistory(
        [FromQuery] Guid? productId,
        [FromQuery] Guid? warehouseId,
        [FromQuery] string? type,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new GetMovementHistoryQuery
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            Type = type,
            StartDate = startDate,
            EndDate = endDate,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpGet("warehouse/{warehouseId}/stock")]
    public async Task<IActionResult> GetStockByWarehouse(Guid warehouseId)
    {
        var result = await _mediator.Send(new GetStockByWarehouseQuery(warehouseId));

        if (result.IsFailure)
            return NotFound(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpPost("purchase")]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<IActionResult> ProcessPurchase([FromBody] ProcessPurchaseCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpPost("sale")]
    [Authorize(Roles = "Admin,Supervisor,Operator")]
    public async Task<IActionResult> ProcessSale([FromBody] ProcessSaleCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpPost("transfer")]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<IActionResult> TransferStock([FromBody] TransferStockCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }
}