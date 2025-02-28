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
AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);

await channel.BasicConsumeAsync(
    queue: queueName,
    autoAck: false,
    consumer: consumer);
consumer.ReceivedAsync += async (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
};
#endregion
*/
/*
#region PublistSubcriberTasarımı

string exchangeName = "pub-sub-exchange";
await channel.QueueDeclareAsync(queue: "pub-sub-queue");
await channel.QueueBindAsync(
    queue:"pub-sub-queue",
    exchange: exchangeName,
    routingKey:String.Empty);
AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicConsumeAsync(
    queue:"pub-sub-queue",
    autoAck: false,
    consumer: consumer);
consumer.ReceivedAsync += async (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
};
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
AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicConsumeAsync(
    queueName, 
    autoAck:true, 
    consumer: consumer);
await channel.BasicQosAsync( //tüm consumerlar aynı iş yüküne ve görev dağılımına sahip olmasını sağlar
    prefetchCount:1, //aynı anda bir mesaj işler
    prefetchSize: 0, // sınırsız byte'ta mesaj alabilir
    global: false);
consumer.ReceivedAsync += async (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
};
#endregion
*/

#region Request Response Tasarımı

string requestQueueName = "request-response-queue";
await channel.QueueDeclareAsync(
    queue: requestQueueName,
    durable: false,
    exclusive: false,
    autoDelete: false);
AsyncEventingBasicConsumer consumer = new AsyncEventingBasicConsumer(channel);
await channel.BasicConsumeAsync(
    queue:requestQueueName,
    consumer: consumer,
    autoAck:true);
consumer.ReceivedAsync += async (sender, e) =>
{
    Console.WriteLine(Encoding.UTF8.GetString(e.Body.ToArray()));
    byte[] responseMessage = Encoding.UTF8.GetBytes("Mesajını Aldım ve İşlemini Tamamladım Haberin Ola");
    var properties = new BasicProperties();
    properties.CorrelationId = e.BasicProperties.CorrelationId;
    await channel.BasicPublishAsync(
        exchange: String.Empty,
        routingKey: e.BasicProperties.ReplyTo,
        basicProperties: properties,
        mandatory: true,
        body: responseMessage);
};

#endregion
Console.Read();