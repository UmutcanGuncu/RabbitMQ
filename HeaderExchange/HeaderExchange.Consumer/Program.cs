using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh");

using IConnection connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
    exchange: "header-exchange-example",
    type: ExchangeType.Headers);
await channel.QueueDeclareAsync(queue: "isimsizKuyruk");
await channel.QueueBindAsync(
    queue:"isimsizKuyruk",
    exchange: "header-exchange-example",
    routingKey: String.Empty,
    new Dictionary<string, object?>
    {
        ["umutcan"] = "guncu"
    });
AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicConsumeAsync(
    queue: "isimsizKuyruk",
    autoAck: true,
    consumer: consumer);
consumer.ReceivedAsync += async (sender, e) =>
{
    string messageBody = Encoding.UTF8.GetString(e.Body.ToArray());
    Console.WriteLine($"Received message: {messageBody}");
};
Console.Read();