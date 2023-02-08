using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaxiDispacher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpPost("Create")]
        async public Task<IActionResult> Post(OrdersModel orders)
        {
            return Ok();
        }

        [HttpGet("Detail")]
        async public Task<IActionResult> Get(int orderId)
        {
            return Ok();
        }

        [Authorize(Roles = UsersModel.ROLE_DRIVER)]
        [HttpPut("Picked")]
        async public Task<IActionResult> Pickup(int orderId)
        {
            return Ok();
        }

        [Authorize(Roles = UsersModel.ROLE_ADMIN)]
        [HttpPut("Assign")]
        async public Task<IActionResult> Assign(int orderId, int driverId)
        {
            return Ok();
        }

        [Authorize(Roles = UsersModel.ROLE_ADMIN)]
        [HttpPut("Cancel")]
        async public Task<IActionResult> Cancel(int orderId)
        {
            return Ok();
        }

        [Authorize]
        [HttpPut("Finish")]
        async public Task<IActionResult> Finish(int orderId)
        {
            return Ok();
        }
    }
}
