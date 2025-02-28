using MassTransit;
using RabbitMQ.ESB.MassTransit.RequestResponse.Consumer.Consumers;

string rabbitMQUri = "amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
    factory.ReceiveEndpoint("request-queue", endpoint =>
    {
        endpoint.Consumer<RequestMessageConsumer>();
    });
});
await bus.StartAsync();

Console.Read();