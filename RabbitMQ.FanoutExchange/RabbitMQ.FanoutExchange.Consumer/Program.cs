using System.Runtime.Loader;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh");

using IConnection connetion = await factory.CreateConnectionAsync();
using IChannel channel = await connetion.CreateChannelAsync();

await channel.ExchangeDeclareAsync(
    exchange:"fanout-exchange",
    type:ExchangeType.Fanout);
Console.WriteLine("Kuyruk Adını Giriniz : ");
string  queueName = Console.ReadLine();
await channel.QueueDeclareAsync(
    queue: queueName,
    exclusive:false);
await channel.QueueBindAsync( // exchange ile kuyruk arasında iletişim kurmuş olduk
    queue:queueName,
    exchange: "fanout-exchange",
    routingKey:string.Empty);
AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicConsumeAsync(
    queue: queueName,
    autoAck: true,
    consumer: consumer);
consumer.ReceivedAsync += async (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
};

Console.Read();