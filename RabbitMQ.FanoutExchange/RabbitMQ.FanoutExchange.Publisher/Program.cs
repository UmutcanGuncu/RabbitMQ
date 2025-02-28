
using System.Text;
using RabbitMQ.Client;
// ilgili exchange e bind olmuş tüm kuyruklar mesajı alır
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh");

using IConnection connetion = await factory.CreateConnectionAsync();
using IChannel channel = await connetion.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: "fanout-exchange", type: ExchangeType.Fanout);
for (int i = 0; i < 100; i++)
{
    await Task.Delay(250);
    byte[] message = Encoding.UTF8.GetBytes($"Selam {i}");
    await channel.BasicPublishAsync(
        exchange: "fanout-exchange",
        routingKey: string.Empty, // routing key'den ayırt etmeyeceğimiz için boş geçiyoruz
        body: message);
}
Console.Read();