using MassTransit;
using RabbitMQ.ESB.MassTransit.Consumer.Consumers;

string rabbitMQUri = "amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh";

string queueName = "masstransitQueue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri); 
    factory.ReceiveEndpoint(queueName:queueName, endpoint =>
    {
        endpoint.Consumer<ExampleMessageConsumer>();
    }); // consumer konfigürasyonu
});
await bus.StartAsync();
Console.ReadLine(); 