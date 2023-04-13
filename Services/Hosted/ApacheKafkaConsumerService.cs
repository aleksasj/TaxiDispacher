using Confluent.Kafka;
using Serilog;
using System.Diagnostics;
using System.Text.Json;
using System.Text.RegularExpressions;
using TaxiDispacher.Controllers.Forms;

namespace TaxiDispacher.Services.Hosted;

public class ApacheKafkaConsumerService : IHostedService
{
    private ConsumerConfig _config;
    private Task task;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ApacheKafkaConsumerService> _logger;
    private readonly IOrderService _orderService;

    public ApacheKafkaConsumerService(IConfiguration configuration, ILogger<ApacheKafkaConsumerService> logger, IOrderService orderService)
    {
        _configuration = configuration;
        _logger = logger;
        _orderService = orderService;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _config = new ConsumerConfig {   
            GroupId = _configuration.GetValue<String>("kafka:groupId", "group"),
            BootstrapServers = _configuration.GetValue<String>("kafka:server", "localhost:9092"),
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        task = Task.Run(() => ExecuteTaskAsync(_config));

        return task;
    }

    private async Task ExecuteTaskAsync(ConsumerConfig config)
    {
        _logger.LogInformation("Consumer kafka started");
        try
        {
            using var consumerBuilder = new ConsumerBuilder <Ignore, string>(config).Build();
            
            consumerBuilder.Subscribe("order:create");
            var cancelToken = new CancellationTokenSource();

            try
            {
                while (true)
                {
                    var consumer = consumerBuilder.Consume(cancelToken.Token);
                    var orderRequest = JsonSerializer.Deserialize<OrderCreateForm>(consumer.Message.Value);
                    if (orderRequest != null)
                    {
                        _logger.LogInformation("Order received from kafka event");
                        var order = await _orderService.Create(orderRequest);
                        if (order != null)
                        {
                            _logger.LogInformation("Order " + order.Id + " CREATED using kafka");
                        }
                    }
                }
                    
            } catch (OperationCanceledException e)
            {
                consumerBuilder.Close();
                _logger.LogError(e.Message);
            }
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
        }
    }

    public Task StopAsync(CancellationToken stoppingToken)
    {
        task?.Dispose();

        _logger.LogInformation("Consumer kafka stoped");

        return Task.CompletedTask;
    }

}
