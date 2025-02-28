using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

string rabbitMQUri = "amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh";

string queueName = "masstransitQueue";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
   factory.Host(rabbitMQUri); 
});
var sendEndPoint = await bus.GetSendEndpoint(new ($"{rabbitMQUri}/{queueName}"));

Console.Write("Gönderilecek Mesaj");
string message = Console.ReadLine();
await sendEndPoint.Send<IMessage>(new ExampleMessage()
{
   Text = message
});
Console.Read();