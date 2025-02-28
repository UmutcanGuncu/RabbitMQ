
using System.Text;
using RabbitMQ.Client;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh");

using IConnection connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
    exchange:"topic-exchange",
    type:ExchangeType.Topic);

for (int i = 0; i < 100; i++)
{
    await Task.Delay(250);
    byte [] message = Encoding.UTF8.GetBytes($"Topic Exchange Örneği {i} ");
    await channel.BasicPublishAsync(
        exchange: "topic-exchange",
        routingKey: "ahmet.mehmet.zübeyir",
        body: message);
}
Console.Read();