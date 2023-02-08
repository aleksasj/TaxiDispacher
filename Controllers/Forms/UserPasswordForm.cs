using System.ComponentModel.DataAnnotations;

namespace TaxiDispacher.Controllers.Forms;

public class UserPasswordForm
{
    [Required]
    [MinLength(8)]
    public string OldPassword { get; set; }
    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; }
}
