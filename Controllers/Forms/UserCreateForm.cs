using System.ComponentModel.DataAnnotations;

namespace TaxiDispacher.Controllers.Forms;

public class UserCreateForm
{
    [Required]
    [MinLength(5)]
    public string Username { get; set; }
    [Required]
    [MinLength(8)]
    public string Password { get; set; }
}
