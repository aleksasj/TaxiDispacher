using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace TaxiDispacher.Services.Hosted
{
    public class DispatchOrderService : IHostedService, IDisposable
    {
        private Timer? _timer = null;
        private readonly IOrderService _orderService;

        public DispatchOrderService(IOrderService orderService)
        {
            _orderService = orderService;
        }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _timer = new Timer(ExecuteTask, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));

            return Task.CompletedTask;
        }

        private void ExecuteTask(object? state)
        {
            _orderService.AssignForAvailableDrivers();
            _orderService.CancelPendingTooLong();
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
