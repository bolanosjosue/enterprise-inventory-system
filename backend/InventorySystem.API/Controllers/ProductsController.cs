using InventorySystem.Application.Products.Commands.CreateProduct;
using InventorySystem.Application.Products.Queries.GetProductById;
using InventorySystem.Application.Products.Queries.GetProducts;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts(
        [FromQuery] string? search,
        [FromQuery] Guid? categoryId,
        [FromQuery] string? status,
        [FromQuery] bool? lowStock,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var query = new GetProductsQuery
        {
            Search = search,
            CategoryId = categoryId,
            Status = status,
            LowStock = lowStock,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        var result = await _mediator.Send(new GetProductByIdQuery(id));

        if (result.IsFailure)
            return NotFound(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Supervisor")]
    public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return CreatedAtAction(nameof(GetProductById), new { id = result.Value.Id }, result.Value);
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStockProducts()
    {
        var query = new GetProductsQuery { LowStock = true, PageSize = 100 };
        var result = await _mediator.Send(query);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value.Items);
    }
}