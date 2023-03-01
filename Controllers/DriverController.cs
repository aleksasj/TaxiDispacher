using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaxiDispacher.Services;

namespace TaxiDispacher.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DriverController : ControllerBase
{
    public readonly IDriverService _driverService;

    public DriverController(IDriverService driverService)
    {
        _driverService = driverService;
    }

    [Authorize(Roles = UsersModel.ROLE_DRIVER)]
    [HttpPost("Location")]
    async public Task<IActionResult> ShareLocation([FromForm] float longitude, [FromForm] float latitude)
    {
        await _driverService.AddLocation(longitude, latitude);

        return NoContent();
    }

    [Authorize(Roles = UsersModel.ROLE_DRIVER)]
    [HttpGet("Orders")]
    async public Task<IActionResult> Orders(int page = 1, int perPage = 10, int[] status = null)
    {
        var result = await _driverService.GetOrders(page, status, perPage);

        return Ok(result);
    }

    [Authorize(Roles = UsersModel.ROLE_DRIVER)]
    [HttpPost("StopWorking")]
    async public Task<IActionResult> StopWorking()
    {
        bool success = await _driverService.StopWorking();

        return success ? Ok() : BadRequest();
    }

    [Authorize(Roles = UsersModel.ROLE_DRIVER)]
    [HttpPost("StartWorking")]
    async public Task<IActionResult> StartWorking()
    {
        bool success = await _driverService.StartWorking();

        return success ? Ok() : BadRequest();
    }
}
