using InventorySystem.Application.Authentication.Commands.UpdateUserRole;
using InventorySystem.Application.Authentication.Commands.ToggleUserStatus;
using InventorySystem.Application.Authentication.Queries.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.API.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var result = await _mediator.Send(new GetUsersQuery());

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpPut("{id}/role")]
    public async Task<IActionResult> UpdateUserRole(Guid id, [FromBody] UpdateUserRoleCommand command)
    {
        if (id != command.UserId)
            return BadRequest(new { error = "ID mismatch" });

        var result = await _mediator.Send(command);

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }

    [HttpPut("{id}/toggle-status")]
    public async Task<IActionResult> ToggleUserStatus(Guid id)
    {
        var result = await _mediator.Send(new ToggleUserStatusCommand(id));

        if (result.IsFailure)
            return BadRequest(new { error = result.Error });

        return Ok(result.Value);
    }
}