using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory   = new ConnectionFactory();
factory.Uri = new Uri("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh");


using IConnection connection = await factory.CreateConnectionAsync();
using var channel = await connection.CreateChannelAsync();
/*
#region P2P Tasarımı
string queueName = "p2p-queue";
await channel.QueueDeclareAsync(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false);
byte [] message = Encoding.UTF8.GetBytes("Bu çağrı tüm otobotlara");

await channel.BasicPublishAsync(
    exchange: String.Empty,
    routingKey:queueName,
    body:message
    );

#endregion
*/
/*
#region Publish/SubcriberTasarımı
string exchangeName = "pub-sub-exchange";
await channel.ExchangeDeclareAsync(
    exchange: exchangeName,
    type: ExchangeType.Fanout);
for (int i = 0; i < 100; i++)
{
    Task.Delay(500);
    byte[] message = Encoding.UTF8.GetBytes($"Selam {i}");
    await channel.BasicPublishAsync(
        exchange: exchangeName,
        routingKey: String.Empty,
        body: message);

}


#endregion
*/
/*
#region Work Queue Tasarımı

string queueName = "workQueue";
await channel.QueueDeclareAsync(
    queue: queueName,
    durable: false,
    exclusive: false,
    autoDelete: false);
for (int i = 0; i < 100; i++)
{
    await Task.Delay(250);
    byte[] message = Encoding.UTF8.GetBytes($"Bu Bir Mesajdır (Halla Halla) {i}");
    await channel.BasicPublishAsync(
        exchange: String.Empty,
        routingKey: queueName,
        body: message);
    
}

#endregion
*/

#region Request Response Tasarımı
// Publisher ve consumer sınıflar bu tasarımda iki işlemi birlikte yürütmektedir.
// Özetle hem mesaj gönderip hem de mesaj alabilmektedirler.
string requestQueueName = "request-response-queue";
await channel.QueueDeclareAsync(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);
await channel.QueueDeclareAsync(queue: "reply-queue"); // response queue yi oluşturuyoruz
string correlationId = Guid.NewGuid().ToString(); //gönderilen request'in response'ı olup olmadığını oluşturmuş olduğumuz id ile karşılaştıracaz

#region Request Mesajı Oluşturup Gönderme
var properties = new BasicProperties();
properties.CorrelationId = correlationId; // karşılaştırmayı yapacağımız id bilgisini veriyoruz
properties.ReplyTo = "reply-queue"; // sonuç mesajının hangi kuyruğa gönderileceğini ifade eder

for (int i = 0; i < 100; i++)
{
    await Task.Delay(250);
    byte [] mesasge = Encoding.UTF8.GetBytes($"Bu Bir Request {i}");
    await channel.BasicPublishAsync(
        exchange:String.Empty,
        routingKey:requestQueueName,
        body:mesasge,
        basicProperties: properties,
        mandatory:true);
}
#endregion

#region Response Kuyruğu Dinleme
AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicConsumeAsync(
    queue:"reply-queue",
    autoAck: true,
    consumer: consumer);
consumer.ReceivedAsync += async (sender, e) =>
{
    if (e.BasicProperties.CorrelationId == correlationId)
    {
        Console.WriteLine($"Response {Encoding.UTF8.GetString(e.Body.ToArray())}");
    }
};

#endregion
#endregion
Console.Read();