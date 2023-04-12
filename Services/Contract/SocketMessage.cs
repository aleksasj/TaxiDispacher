namespace TaxiDispacher.Services.Contract;

public class SocketMessage
{
    public SocketMessage(string topic, object payload)
    {
        Topic = "";
        Payload = payload;
    }
    
    public string Topic { get; set; }
    public object Payload { get; set; }
}
