
using TaxiDispacher.Controllers.Forms;

namespace TaxiDispacher.Services;

public interface IOrderService
{
    Task<bool> Create(OrderCreateForm order);
    Task<bool> Assign(int orderId, int? driverId);
    Task<bool> Cancel(int orderId);
    Task<OrdersModel?> Details(int orderId);
    Task<bool> Finish(int orderId);
    Task<bool> Pickup(int orderId);
}

public class OrderService : IOrderService
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

    public Task<bool> Assign(int orderId, int? driverId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Cancel(int orderId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Create(OrderCreateForm order)
    {
        throw new NotImplementedException();
    }

    public Task<OrdersModel?> Details(int orderId)
    {
        return null;
    }

    public Task<bool> Finish(int orderId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Pickup(int orderId)
    {
        throw new NotImplementedException();
    }
}
