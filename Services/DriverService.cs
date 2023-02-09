namespace TaxiDispacher.Services;

public class DriverService : IDriverService
{
    private readonly IConfiguration _config;
    private readonly IUserService _userService;
    private readonly IDriverRepository _driverRepository;

    public DriverService(IConfiguration config, IUserService userService, IDriverRepository driverRepository)
    {
        _config = config;
        _userService = userService;
        _driverRepository = driverRepository;
    }

    public async Task AddLocation(float latitude, float longitude)
    {
        await _driverRepository.AddLocation(_userService.GetUser().Id, latitude, longitude);
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
}