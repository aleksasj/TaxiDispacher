using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaxiDispacher.Controllers.Forms;
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
            var token = await _userService.GetToken(username, password);

            return token == null ? NotFound() : Ok(token);
        }

        [Authorize]
        [HttpPut("Password")]
        public async Task<IActionResult> Password([FromForm] UserPasswordForm passwordChange)
        {
            var success = await _userService.ChangePassword(passwordChange.OldPassword, passwordChange.NewPassword);

            return success ? Ok() : NotFound();
        }

        [Authorize(Roles = UsersModel.ROLE_ADMIN)]
        [HttpPost("Create")]
        public async Task<IActionResult> Create([FromForm] UserCreateForm user)
        {
            var success = await _userService.Create(user.Username, user.Password);

            return success ? Ok() : NotFound();
        }

    }
}