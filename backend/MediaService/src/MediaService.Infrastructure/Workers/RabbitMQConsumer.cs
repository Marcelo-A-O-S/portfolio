using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
namespace MediaService.Infrastructure.Workers
{
    public class RabbitMQConsumer
    {
        private readonly IConnection connection;
        private readonly Dictionary<string, Func<string, Task>> handlers = new();
        public RabbitMQConsumer(
            IConnection _connection)
        {
            this.connection = _connection;
        }
        public void RegisterHandler(string eventName, Func<string, Task> _handler)
        {
            this.handlers[eventName] = _handler;
        }
        public async Task Start()
        {
            try
            {
                foreach (var eventName in this.handlers.Keys)
                {
                    string exchangeName = $"{eventName}.Exchange";
                    string queueName = $"{eventName}.Queue";
                    string routingKey = $"{eventName}.Routing";
                    var channel = await this.connection.CreateChannelAsync();
                    await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct, durable: true, autoDelete: false);
                    var queue = await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false);
                    await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: routingKey);
                    await channel.BasicQosAsync( prefetchSize: 0, prefetchCount: 10, global: false);
                    var consumer = new AsyncEventingBasicConsumer(channel);
                    consumer.ReceivedAsync += async (model, ea) =>
                    {
                        try
                        {
                            var body = Encoding.UTF8.GetString(ea.Body.ToArray());
                            await handlers[eventName](body);
                            await channel.BasicAckAsync(deliveryTag: ea.DeliveryTag, multiple: false);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Erro ao processar handler: {ex.Message}");
                            await channel.BasicNackAsync(deliveryTag: ea.DeliveryTag, multiple: false, requeue: false);
                        }
                    };
                    await channel.BasicConsumeAsync(queue: queue, autoAck: false, consumer: consumer);
                    Console.WriteLine($"Consumer ativo para o evento '{eventName}' usando fila '{queueName}'");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}