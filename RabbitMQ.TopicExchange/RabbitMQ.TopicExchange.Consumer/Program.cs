using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh");

using IConnection connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
    exchange:"topic-exchange",
    type:ExchangeType.Topic);
Console.WriteLine("Dinelenecek Topic Formatını Belirt");
string topic = Console.ReadLine();

await channel.QueueDeclareAsync(queue: "topic-queue*.*");
await channel.QueueBindAsync(
    queue:"topic-queue",
    exchange:"topic-exchange",
    routingKey: topic);
AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicConsumeAsync(
    queue:"topic-queue",
    autoAck: true,
    consumer: consumer);
consumer.ReceivedAsync += async (sender, e) =>
{
    Console.WriteLine($"Got message: {Encoding.UTF8.GetString(e.Body.Span)}");
}; 
Console.Read();