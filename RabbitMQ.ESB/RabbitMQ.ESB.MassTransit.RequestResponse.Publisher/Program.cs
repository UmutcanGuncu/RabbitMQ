using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.RequestResponseMessages;

string rabbitMQUri = "amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh";

IBusControl bus = Bus.Factory.CreateUsingRabbitMq(factory =>
{
    factory.Host(rabbitMQUri);
});
await bus.StartAsync();
var requestClient = bus.CreateRequestClient<RequestMessage>(new Uri($"{rabbitMQUri}/request-queue"));

int i = 0;
while (true)
{
    await Task.Delay(200);
    var response = await requestClient.GetResponse<ResponseMessage>(new RequestMessage { MessageNo = ++i, Text = $"{i}. Request Budur"});
    Console.WriteLine(response.Message.Text);
}
Console.Read();