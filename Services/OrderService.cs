
namespace TaxiDispacher.Services;

public class OrderService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IOrderRepository _orderRepository;

    public OrderService(IConfiguration config, IUserService userService, IUserRepository userRepository, IOrderRepository orderRepository) 
    {
        _config = config;
        _userRepository = userRepository;
        _userService = userService;
    }
   
}
