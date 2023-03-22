using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaxiDispacher.Helpers;

namespace TaxiDispacher.Services;

public interface IUserService
{
    public UsersModel? GetUser();
    public Task<string?> GetToken(string username, string password);
    public Task<bool> ChangePassword(string oldPassword, string newPassword);
    public Task<bool> Create(string username, string password);
}

public class UserService : IUserService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _request;
    private readonly ILogger<UserService> _logger;
    private string? _securitySalt = string.Empty;

    public UserService(IConfiguration config, ILogger<UserService> logger, IUserRepository userRepository, IHttpContextAccessor request)
    {
        _config = config;
        _userRepository = userRepository;
        _request = request;
        _logger = logger;
        _securitySalt = _config.GetValue<String>("Salt");
    }
    public UsersModel? GetUser()
    {
        var currentUser = _request.HttpContext.User.Claims;

        if (currentUser != null)
        {
            return new UsersModel()
            {
                Id = Int32.Parse(currentUser.FirstOrDefault(u => u.Type == "UID").Value),
                Username = currentUser.FirstOrDefault(u => u.Type == ClaimTypes.NameIdentifier).Value,
                Password = currentUser.FirstOrDefault(u => u.Type == "Passowrd").Value,
                Role = currentUser.FirstOrDefault(u => u.Type == ClaimTypes.Role).Value,
            };
        }
        return null;
    }

    async public Task<string?> GetToken(string username, string password)
    {
        password = Crypt.Encrypt(password, _securitySalt);
        var user = await _userRepository.GetUserByCredentials(username, password);

        return user == null ? null : GenerateToken(user);
    }
    public async Task<bool> ChangePassword(string oldPassword, string newPassword)
    {
        oldPassword = Crypt.Encrypt(oldPassword, _securitySalt);
        if (oldPassword == GetUser().Password)
        {
            newPassword = Crypt.Encrypt(newPassword, _securitySalt);
            await _userRepository.ChangePassword(GetUser().Id, newPassword);
            return true;

        }

        return false;
    }

    public async Task<bool> Create(string username, string password)
    {
        password = Crypt.Encrypt(password, _securitySalt);
        try
        {
            await _userRepository.Create(username, password);
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    private string GenerateToken(UsersModel user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var claims = new[]
        {
            new Claim("UID", user.Id + ""),
            new Claim("Passowrd", user.Password),
            new Claim(ClaimTypes.NameIdentifier, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

        var token = new JwtSecurityToken(_config["Jwt:Issuer"],
            _config["Jwt:Audience"],
            claims,
            expires: DateTime.Now.AddMinutes(15),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
