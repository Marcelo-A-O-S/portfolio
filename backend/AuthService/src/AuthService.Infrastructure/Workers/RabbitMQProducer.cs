using AuthService.Domain.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace AuthService.Infrastructure.Workers
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IConnectionFactory factory;
        public RabbitMQProducer(IConnectionFactory _factory)
        {
            this.factory = _factory;
        }
        public async Task Publish(string eventName, object data)
        {
            using var connection = await this.factory.CreateConnectionAsync();
            using var channel = await connection.CreateChannelAsync();
            await channel.ExchangeDeclareAsync(exchange: eventName, type: ExchangeType.Direct, durable: true);
            var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(data));
            await channel.BasicPublishAsync(exchange: eventName, routingKey: eventName, body: body);
        }
    }
}