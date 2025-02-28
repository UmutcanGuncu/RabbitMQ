using System.Text;
using RabbitMQ.Client;

//Bağlantı Oluşturma
ConnectionFactory factory = new ConnectionFactory();
factory.Uri = new Uri("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh");

//Bağlantıyı Aktifleştirme ve Kanal Açma
using IConnection connection =  await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: "direct-exchange-example", type: ExchangeType.Direct); 
//Exchange'in tipini type parametresinden ExchangeType sınıfını kullanarak belirtiriz
while (true)
{
    Console.WriteLine("Enter your message:");
    var message = Console.ReadLine();
    byte[] messageBytes = Encoding.UTF8.GetBytes(message);
    await channel.BasicPublishAsync(
        exchange:"direct-exchange-example",
        routingKey:"direct-queue-example",
        body: messageBytes);
    
}
Console.Read();