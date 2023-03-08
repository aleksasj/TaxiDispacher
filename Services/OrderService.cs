
using TaxiDispacher.Controllers.Forms;

namespace TaxiDispacher.Services;

public interface IOrderService
{
    Task<OrderListModel?> Create(OrderCreateForm order);
    Task<bool> Assign(int orderId, int? driverId = null);
    Task<bool> Cancel(int orderId);
    Task<OrderListModel?> Details(int orderId);
    Task<bool> Finish(int orderId);
    Task<bool> Pickup(int orderId);
    Task AssignForAvailableDrivers();
    Task CancelPendingTooLong();
}

public class OrderService : IOrderService
{
    private readonly IUserService _userService;
    private readonly IOrderRepository _orderRepository;
    private readonly IAddressRepository _addressRepository;
    private readonly IDriverRepository _driverRepository;
    private readonly ILogger<OrderService> _logger;
    private readonly IConfiguration _config;


    public OrderService(IConfiguration config, ILogger<OrderService> logger, IUserService userService, IOrderRepository orderRepository, IAddressRepository addressRepository, IDriverRepository driverRepository)
    {
        _config = config;
        _userService = userService;
        _logger = logger;
        _orderRepository = orderRepository;
        _addressRepository = addressRepository;
        _driverRepository = driverRepository;
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

    public async Task AssignForAvailableDrivers()
    {
        int maxDistance = _config.GetValue<int>("order:maxDistance", 10);
        int maxOrderCount = _config.GetValue<int>("order:maxOrderCount", 0);
        IEnumerable<OrderListModel> orderLists = await _orderRepository.getPendingList();
        foreach (var item in orderLists)
        {
            IEnumerable<UsersModel> availableDrivers = await _driverRepository.getAvailable(item.PickupLatitude, item.PickupLongitude, maxOrderCount, maxDistance);

            var firstDriver = availableDrivers.FirstOrDefault();
            if (firstDriver == null)
            {
                _logger.LogInformation("No driver available for order: " + item.Id);
                continue;
            }

            await _orderRepository.Assign(item.Id, firstDriver.Id);
            _logger.LogInformation("Driver " + firstDriver.Id + " was assigned for " + item.Id);
            _logger.LogInformation("Total available driver count for this order:" + availableDrivers.Count());
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

    public async Task CancelPendingTooLong() => await _orderRepository.CancelPendingTooLong(_config.GetValue<int>("order:cancelTime", 5));

    public async Task<OrderListModel?> Create(OrderCreateForm order)
    {
        try
        {
            var pickupAddress = await _addressRepository.GetOrCreate(order.Pickup.Title, order.Pickup.Longitude, order.Pickup.Latitude);
            var destinationAddress = await _addressRepository.GetOrCreate(order.Destination.Title, order.Destination.Longitude, order.Destination.Latitude);

            var orderData = await _orderRepository.Create(order.Name, order.Phone, pickupAddress.Id, destinationAddress.Id, order.Comment);

            return orderData;
        } catch(Exception e)
        {
            _logger.LogError(e.Message);
        }

        return null;
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
