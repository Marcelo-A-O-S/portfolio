namespace CommentService.Application.Interfaces
{
    public interface IRabbitMQProducer
    {
        Task Publish(string eventName, object data);
    }
}