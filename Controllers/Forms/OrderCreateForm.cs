using System.ComponentModel.DataAnnotations;

namespace TaxiDispacher.Controllers.Forms;

public class OrderCreateForm
{
    [Required]
    [MinLength(2)]
    public string Name { internal get; set; }
    [Required]
    [MinLength(7)]
    public string Phone { get; set; }
    [Required]
    public OrderAddressForm? Pickup { get; set; }
    [Required]
    public OrderAddressForm? Destination { get; set; }

    [Required]
    public string Comment { get; set; }
}

public class OrderAddressForm
{
    public string Title { get; set; }
    public float Latitude { get; set; }
    public float Longitude { get; set; }
}
