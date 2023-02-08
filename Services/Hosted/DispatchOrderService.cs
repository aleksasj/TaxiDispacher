using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TaxiDispacher.Services.Hosted
{
    public class DispatchOrderService : IHostedService, IDisposable
    {
        private readonly UserService _authService;
        private Timer? _timer = null;

        public DispatchOrderService(IServiceScopeFactory scopeFactory)
        {
            //_authService = scopeFactory.CreateScope().ServiceProvider.GetRequiredService<AuthService>();

        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void ExecuteTask(object? state)
        {

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
