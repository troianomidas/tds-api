namespace WebApi.Integrations.Queues;

public interface IQueue
{
    Task SendMessageAsync(string queue, string message);
    Task CreateQueueAsync(string queue);
}