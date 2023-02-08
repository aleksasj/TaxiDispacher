using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TaxiDispacher.DTO;

public class AuthCreateForm
{
    private string _role = UsersModel.ROLE_DRIVER;

    [Required]
    [MinLength(5)]
    public string Username { get; set; }
    [Required]
    [MinLength(8)]
    public string Password { get; set; }

    [Required]
    public string Role { 
        get
            {
                return _role;
            } 
        set
            {
                _role = value;
            } 
    }
}
