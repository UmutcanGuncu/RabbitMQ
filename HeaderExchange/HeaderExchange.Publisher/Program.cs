using System.Text;
using RabbitMQ.Client;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh");

using IConnection connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
    exchange: "header-exchange-example",
    type: ExchangeType.Headers);
for (int i = 1; i <= 100; i++)
{
    await Task.Delay(250);
    byte [] message = Encoding.UTF8.GetBytes($"Hello World {i}");

    var basicProperties = new BasicProperties();
    basicProperties.Headers = new Dictionary<string, object?>()
    {
        ["umutcan"] = "guncu"
    };
    await channel.BasicPublishAsync(
        exchange:"header-exchange-example",
        routingKey:String.Empty,
        body: message,
        basicProperties:basicProperties,
        mandatory:true);
}
Console.Read();