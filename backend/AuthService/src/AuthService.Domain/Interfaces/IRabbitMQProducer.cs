namespace AuthService.Domain.Interfaces
{
    public interface IRabbitMQProducer
    {
        Task Publish(string eventName, object data);
    }
}