
using TaxiDispacher.Controllers.Forms;

namespace TaxiDispacher.Services;

public interface IOrderService
{
    Task<bool> Create(OrderCreateForm order);
    Task<bool> Assign(int orderId, int? driverId = null);
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
    private readonly ILogger<OrderService> _logger;

    public OrderService(IConfiguration config, ILogger<OrderService> logger, IUserService userService, IUserRepository userRepository, IOrderRepository orderRepository)
    {
        _config = config;
        _userRepository = userRepository;
        _userService = userService;
        _logger = logger;
    }

    public async Task<bool> Assign(int orderId, int? driverId = null)
    {
        try
        {
            var did = (driverId != null ? driverId : _userService.GetUser().Id);
            await _orderRepository.Assign(orderId, did);
            return true;
        } catch (Exception ex) {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    public async Task<bool> Cancel(int orderId)
    {
        try
        {
            await _orderRepository.Cancel(orderId);

            return true;
        }
        catch (Exception ex) {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    public async Task<bool> Create(OrderCreateForm order)
    {
        throw new NotImplementedException();
    }

    public async Task<OrdersModel?> Details(int orderId) => await _orderRepository.Detail(orderId);

    public async Task<bool> Finish(int orderId)
    {
        try
        {
            await _orderRepository.Finish(orderId);

            return true;
        }
        catch (Exception ex) {
            _logger.LogError(ex.Message);
            return false;
        }
    }

    public async Task<bool> Pickup(int orderId)
    {
        try
        {
            await _orderRepository.Picked(orderId);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }
}
