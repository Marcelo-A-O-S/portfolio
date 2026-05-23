using System.Text;
using System.Text.Json;
using PostService.Domain.Interfaces;
using RabbitMQ.Client;
namespace PostService.Infrastructure.Workers
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IConnection connection;
        public RabbitMQProducer(IConnection _connection)
        {
            this.connection = _connection;
        }
        public async Task Publish(string eventName, object data)
        {
            string exchangeName = $"{eventName}-exchange";
            string queueName = $"{eventName}-queue";
            string routingKey = $"{eventName}-routing";
            await using var channel = await connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
            var queue = await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
            await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };
            await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, mandatory: false, basicProperties: properties, body: body);
        }
    }
}