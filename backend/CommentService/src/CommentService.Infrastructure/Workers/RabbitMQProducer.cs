using CommentService.Application.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
namespace CommentService.Infrastructure.Workers
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
            string exchangeName = $"{eventName}.Exchange";
            string routingKey = $"{eventName}.Routing";
            await using var channel = await connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };
            await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, mandatory: false, basicProperties: properties, body: body);
        }
        public async Task PublishRaw(string eventName, string payload)
        {
            string exchangeName = $"{eventName}.Exchange";
            string routingKey = $"{eventName}.Routing";
            var channelOptions = new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true
            );
            await using var channel = await connection.CreateChannelAsync(channelOptions);
            await channel.ExchangeDeclareAsync(
                exchange: exchangeName,
                type: ExchangeType.Direct,
                durable: true);
            var body = Encoding.UTF8.GetBytes(payload);
            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };
            await channel.BasicPublishAsync(
                exchange: exchangeName,
                routingKey: routingKey,
                mandatory: true,
                basicProperties: properties,
                body: body);
        }
    }
}