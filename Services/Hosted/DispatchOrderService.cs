using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TaxiDispacher.Services.Hosted
{
    public class DispatchOrderService : IHostedService, IDisposable
    {
        private readonly UserService _authService;
        private Timer? _timer = null;
        private readonly IUserRepository _userRepository;

        public DispatchOrderService(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            //_authService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<AuthService>();

        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void ExecuteTask(object? state)
        {
            //System.Diagnostics.Debug.WriteLine("Dispatcher running" + _userRepository.Get(1).Result.Username);
        }

        public Task StopAsync(CancellationToken stoppingToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
