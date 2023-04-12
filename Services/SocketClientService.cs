using Newtonsoft.Json;
using System.Net.Sockets;
using System.Text;
using TaxiDispacher.Services.Contract;

namespace TaxiDispacher.Services
{
    public interface ISocketClientService
    {
        Task Send(SocketMessage data);
    }

    public class SocketClientService : ISocketClientService
    {
        private int port;
        private string? ip;

        public SocketClientService(IConfiguration configuration)
        {
            ip = configuration.GetValue<string>("Socker:Ip", "localhost");
            port = configuration.GetValue<int>("Socker:Port", 7067);
        }

        public async Task Send(SocketMessage data)
        {
            using TcpClient client = new TcpClient(ip, port);

            NetworkStream stream = client.GetStream();

            string jsonData = JsonConvert.SerializeObject(data);
            byte[] jsonDataBytes = Encoding.UTF8.GetBytes(jsonData);

            stream.Write(jsonDataBytes, 0, jsonDataBytes.Length);

            stream.Close();
            client.Close();
        }
    }
}
