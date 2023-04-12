using Confluent.Kafka;
using System.Diagnostics;
using System.Net;
using System.Text.Json;

namespace TaxiDispacher.Services;

public interface INotificationsService
{
    void OnDriverLocationChange(int DriverId, float latitude, float longitude);
    void OnDriverStatusChange(int id, bool v);
    void OnOrderCreated(OrderListModel? order);
    void OnOrderStatusChange(int OrderId, int NewStatusId);
}

public class NotificationsService : INotificationsService
{
    private readonly ISocketClientService _socketClient;
    private readonly IConfiguration _configuration;

    public NotificationsService(ISocketClientService socketClient, IConfiguration configuration) 
    {
        _socketClient = socketClient;
        _configuration = configuration;
    }
    public void OnDriverLocationChange(int DriverId, float latitude, float longitude)
    {
        string topic = "DRIVER-LOCATION-UPDATE";
        var objToSend = new
        {
            DriverId,
            Latitude = latitude,
            Longitude = longitude
        };
        Task.Run(() => _socketClient.Send(new Contract.SocketMessage(topic, objToSend)));
        Task.Run(() => SendToKafka(topic, objToSend));
    }

    public void OnDriverStatusChange(int id, bool startWorking)
    {
        string topic = "DRIVER-STATUS-CHANGED#" + id;
        var objToSend = new { IsWorking = startWorking };
        Task.Run(() => _socketClient.Send(new Contract.SocketMessage(topic, objToSend)));
        Task.Run(() => SendToKafka(topic, objToSend));
    }

    public void OnOrderCreated(OrderListModel? order)
    {
        string topic = "ORDER-CREATED#" + order?.Id;
        Task.Run(() => _socketClient.Send(new Contract.SocketMessage(topic, order)));
        Task.Run(() => SendToKafka(topic, topic));
    }

    public void OnOrderStatusChange(int OrderId, int NewStatusId)
    {
        string topic = "ORDER-STATUS-CHANGED#" + OrderId;
        var objToSend = new { Status = NewStatusId };
        Task.Run(() => _socketClient.Send(new Contract.SocketMessage(topic, objToSend)));
        Task.Run(() => SendToKafka(topic, objToSend));
    }

    private async Task SendToKafka(string topic, object message)
    {
        ProducerConfig config = new ProducerConfig
        {
            BootstrapServers = _configuration.GetValue<String>("kafka:server", "localhost:9092"),
            ClientId = Dns.GetHostName()
        };

        try
        {
            using (var producer = new ProducerBuilder <Null, string>(config).Build())
            {
                var result = await producer.ProduceAsync(topic, new Message<Null, string>{
                    Value = JsonSerializer.Serialize(message)
                });
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occured: {ex.Message}");
        }

    }
}
