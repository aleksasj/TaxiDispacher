using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaxiDispacher.DTO;
using TaxiDispacher.Services;

namespace TaxiDispacher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(string username, string password)
        {
            var token = await _userService.getToken(username, password);

            return token == null ? NotFound() : Ok(token);
        }

        [Authorize]
        [HttpPut("Password")]
        public async Task<IActionResult> Password([FromForm] AuthPasswordForm passwordChange)
        {
            var success = await _userService.ChangePassword(passwordChange.OldPassword, passwordChange.NewPassword);

            return success ? Ok() : NotFound();
        }

        [Authorize(Roles = UsersModel.ROLE_ADMIN)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] AuthCreateForm user)
        {
            var success = await _userService.Create(user.Username, user.Password, user.Role);

            return success ? Ok() : NotFound();
        }

    }
}
