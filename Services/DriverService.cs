namespace TaxiDispacher.Services;

public interface IDriverService
{
    Task AddLocation(float latitude, float longitude);
    Task<bool> StartWorking();
    Task<bool> StopWorking();
    Task<IEnumerable<OrderListModel>> GetOrders(int page = 1, int[] status = null, int perPage = 10);
}
public class DriverService : IDriverService
{
    private readonly IConfiguration _config;
    private readonly IUserService _userService;
    private readonly IDriverRepository _driverRepository;
    private readonly IOrderRepository _orderRepository;
    private readonly ILogger<DriverService> _logger;

    public DriverService(IConfiguration config, ILogger<DriverService> logger, IUserService userService, IDriverRepository driverRepository, IOrderRepository orderRepository)
    {
        _logger = logger;
        _config = config;
        _userService = userService;
        _driverRepository = driverRepository;
        _orderRepository = orderRepository;
    }

    public async Task AddLocation(float latitude, float longitude)
    {
        await _driverRepository.AddLocation(_userService.GetUser().Id, latitude, longitude);
    }

    public async Task<IEnumerable<OrderListModel>> GetOrders(int page = 1, int[] status = null, int perPage = 10)
    {
        int? driverId = _userService.GetUser().Id;
        if (_userService.GetUser().Role == UsersModel.ROLE_ADMIN)
        {
            driverId = null;
        }

        return await _orderRepository.GetOrders(driverId, page, status, perPage);
    }

    public async Task<bool> StartWorking()
    {
        try
        {
            await _driverRepository.StartWorking(_userService.GetUser().Id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }
    public async Task<bool> StopWorking()
    {
        try
        {
            await _driverRepository.StopWorking(_userService.GetUser().Id);

            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return false;
        }
    }
}