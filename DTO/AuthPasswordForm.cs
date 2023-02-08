using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace TaxiDispacher.DTO;

public class AuthPasswordForm
{
    [Required]
    [MinLength(8)]
    public string OldPassword { get; set; }
    [Required]
    [MinLength(8)]
    public string NewPassword { get; set; }
}
