using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaxiDispacher.Controllers.Forms;
using TaxiDispacher.Services;

namespace TaxiDispacher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;

        }

        [HttpPost("Create")]
        async public Task<IActionResult> Post([FromBody] OrderCreateForm order)
        {
            bool result = await _orderService.Create(order);

            return result ? Ok() : BadRequest();
        }

        [HttpGet("Detail")]
        async public Task<IActionResult> Get([FromForm] int orderId)
        {
            var result = await _orderService.Details(orderId);

            return result != null ? Ok(result) : NotFound();
        }

        [Authorize(Roles = UsersModel.ROLE_DRIVER)]
        [HttpPut("Picked")]
        async public Task<IActionResult> Pickup([FromForm] int orderId)
        {
            bool result = await _orderService.Pickup(orderId);

            return result ? Ok() : NotFound();
        }

        [Authorize]
        [HttpPut("Assign")]
        async public Task<IActionResult> Assign([FromForm] int orderId, [FromForm] int? driverId = null)
        {
            bool result = await _orderService.Assign(orderId, driverId);

            return result ? Ok() : NotFound();
        }

        [Authorize(Roles = UsersModel.ROLE_ADMIN)]
        [HttpPut("Cancel")]
        async public Task<IActionResult> Cancel([FromForm] int orderId)
        {
            bool result = await _orderService.Cancel(orderId);

            return result ? Ok() : NotFound();
        }

        [Authorize]
        [HttpPut("Finish")]
        async public Task<IActionResult> Finish([FromForm] int orderId)
        {
            bool result = await _orderService.Finish(orderId);

            return result ? Ok() : NotFound();
        }
    }
}
