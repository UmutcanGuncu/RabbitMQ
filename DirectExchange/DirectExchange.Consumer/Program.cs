using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

//Bağlantı Oluşturma
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh");

//Bağlantıyı Aktifleştirme ve Kanal Açma
using IConnection connection =  await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: "direct-exchange-example", type: ExchangeType.Direct); 
await channel.QueueDeclareAsync(queue:"direct-queue-example");
await channel.QueueBindAsync(queue: "direct-queue-example", exchange: "direct-exchange-example",routingKey: "direct-queue-example");

AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicConsumeAsync(queue: "direct-queue-example", 
    autoAck: true, 
    consumer: consumer);
consumer.ReceivedAsync += async (sender, e) =>
{
    string message = Encoding.UTF8.GetString(e.Body.Span);
    Console.WriteLine(message);
};
//1. Adım: Publisher'da ki exchange ile birebir aynı isim ve type'a sahip bir exchange tanımla
//2. Adım: Publisher tarafından routing key'de bulunan değerdeki kuyruğa gönderilen mesajları, kendi oluşturduğumuz kuyruğa
//yönlendirerek tüketmemiz gerekmektedir. Bunun için öncelikle bir kuyruk oluşturulmalıdır.
Console.Read();