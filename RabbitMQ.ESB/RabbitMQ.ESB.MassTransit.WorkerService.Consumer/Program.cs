

using MassTransit;
using RabbitMQ.ESB.MassTransit.WorkerService.Consumer.Consumers;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(configurator =>
        {
            configurator.AddConsumer<ExampleMessageConsumer>();
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh"));
                cfg.ReceiveEndpoint("example-message-queue", e=> e.ConfigureConsumer<ExampleMessageConsumer>(context));
            });
        });
    });

var host = builder.Build();
await host.RunAsync();
