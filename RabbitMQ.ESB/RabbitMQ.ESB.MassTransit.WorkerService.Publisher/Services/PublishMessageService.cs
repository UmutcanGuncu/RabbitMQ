using MassTransit;
using RabbitMQ.ESB.MassTransit.Shared.Messages;

namespace RabbitMQ.ESB.MassTransit.WorkerService.Publisher.Services;

public class PublishMessageService : BackgroundService
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PublishMessageService(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int i = 0;
        while (true)
        {
            ExampleMessage message = new ExampleMessage()
            {
                Text = $"Mesaj  {++i}",
            };
           await _publishEndpoint.Publish(message);
        }
    }
}