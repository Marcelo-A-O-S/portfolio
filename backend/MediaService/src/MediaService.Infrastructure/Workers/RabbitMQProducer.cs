using MediaService.Domain.Interfaces;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace MediaService.Infrastructure.Workers
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
    }
}