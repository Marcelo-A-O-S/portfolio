namespace MediaService.Domain.Interfaces
{
    public interface IRabbitMQProducer
    {
        Task Publish(string eventName, object data);
    }
}