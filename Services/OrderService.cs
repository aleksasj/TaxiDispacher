
using TaxiDispacher.Controllers.Forms;

namespace TaxiDispacher.Services;

public interface IOrderService
{
    Task<bool> Create(OrderCreateForm order);
    Task<bool> Assign(int orderId, int? driverId = null);
    Task<bool> Cancel(int orderId);
    Task<OrderListModel?> Details(int orderId);
    Task<bool> Finish(int orderId);
    Task<bool> Pickup(int orderId);

}

public class OrderService : IOrderService
{
    private readonly IConfiguration _config;
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly IOrderRepository _orderRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly ILogger<OrderService> _logger;

    public OrderService(IConfiguration config, ILogger<OrderService> logger, IUserService userService, IUserRepository userRepository, IOrderRepository orderRepository, IAddressRepository addressRepository)
    {
        _config = config;
        _userRepository = userRepository;
        _userService = userService;
        _logger = logger;
        _orderRepository = orderRepository;
        _addressRepository = addressRepository;
    }

    public async Task<bool> Assign(int orderId, int? driverId = null)
    {
        try
        {
            await _orderRepository.Assign(orderId, (driverId != null ? driverId : _userService.GetUser().Id));
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
        var pk = await _addressRepository.Create("5assdasfasfgsdfhfghgfasf44f56h4", 1.2464525f, 344.5363464636f);


        // _orderRepository.Ce*/

        return true;
    }

    public async Task<OrderListModel?> Details(int orderId) => await _orderRepository.Detail(orderId);

    public async Task<bool> Finish(int orderId)
    {
        try
        {
            await _orderRepository.Finish(orderId);
            return true;
        } catch (Exception ex) {
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
