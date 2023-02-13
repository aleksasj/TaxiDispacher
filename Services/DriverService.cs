namespace TaxiDispacher.Services;

public class DriverService : IDriverService
{
    private readonly IConfiguration _config;
    private readonly IUserService _userService;
    private readonly IDriverRepository _driverRepository;
    private readonly IOrderRepository _orderRepository;

    public DriverService(IConfiguration config, IUserService userService, IDriverRepository driverRepository, IOrderRepository orderRepository)
    {
        _config = config;
        _userService = userService;
        _driverRepository = driverRepository;
        _orderRepository = orderRepository;
    }

    public async Task AddLocation(float latitude, float longitude)
    {
        await _driverRepository.AddLocation(_userService.GetUser().Id, latitude, longitude);
    }

    public async Task<IEnumerable<OrdersModel>> GetOrders(int page = 1, int[] status = null, int perPage = 10) 
        => await _orderRepository.GetOrders(_userService.GetUser().Id, page, status, perPage);


    public async Task<bool> StartWorking()
    {
        try
        {
            await _driverRepository.StartWorking(_userService.GetUser().Id);

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }
    public async Task<bool> StopWorking()
    {
        try
        {
            await _driverRepository.StopWorking(_userService.GetUser().Id);

            return true;
        } catch (Exception ex)
        {
            return false;
        }
    }
}

public interface IDriverService
{
    Task AddLocation(float latitude, float longitude);
    Task<bool> StartWorking();
    Task<bool> StopWorking();
    Task<IEnumerable<OrdersModel>> GetOrders(int page = 1, int[] status = null, int perPage = 10);
}