using Microsoft.AspNetCore.Mvc;

namespace InventorySystem.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected IActionResult HandleResult<T>(Application.Common.Models.Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BadRequest(new { error = result.Error });
    }

    protected IActionResult HandleResult(Application.Common.Models.Result result)
    {
        if (result.IsSuccess)
        {
            return Ok();
        }

        return BadRequest(new { error = result.Error });
    }
}