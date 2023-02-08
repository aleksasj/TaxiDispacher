using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaxiDispacher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriverController : ControllerBase
    {
        [Authorize(Roles = UsersModel.ROLE_DRIVER)]
        [HttpPost("Location")]
        async public Task ShareLocation(float longitude, float latitude)
        {
            //await _driverService.addLocation(longitude, latitude);
        }

        [Authorize(Roles = UsersModel.ROLE_DRIVER)]
        [HttpGet("Orders")]
        async public Task<IActionResult> Orders()
        {
            //var list  await _driverService.getOrders(filter);

            return Ok();
        }

        [Authorize(Roles = UsersModel.ROLE_DRIVER)]
        [HttpPost("StopWorking")]
        async public Task<IActionResult> StopWorking()
        {
            //var list  await _driverService.getOrders(filter);

            return Ok();
        }

        [Authorize(Roles = UsersModel.ROLE_DRIVER)]
        [HttpGet("Orders")]
        async public Task<IActionResult> StartWorking()
        {
            //var list  await _driverService.getOrders(filter);

            return Ok();
        }
    }

}
