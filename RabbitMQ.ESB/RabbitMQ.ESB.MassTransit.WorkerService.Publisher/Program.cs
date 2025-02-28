using MassTransit;
using RabbitMQ.ESB.MassTransit.WorkerService.Publisher.Services;

var builder = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddMassTransit(configurator =>
        {
            configurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri("amqps://rvvkxluh:xZyCL6Ea-2UgGTTGgfLnOSRyXyiZnXN3@hawk.rmq.cloudamqp.com/rvvkxluh"));
            });
        });
        services.AddHostedService<PublishMessageService>(provider =>
        {
            using var scope = provider.CreateScope();
            IPublishEndpoint publishEndpoint =  scope.ServiceProvider.GetService<IPublishEndpoint>();
            return new (publishEndpoint);
        });
    });

var host = builder.Build();
await host.RunAsync();