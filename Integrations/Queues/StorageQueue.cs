// using System.Text;
// using Azure.Storage.Queues;
// using Newtonsoft.Json;
//
// namespace WebApi.Integrations.Queues;
//
// public class StorageQueue : IQueue
// {
//     private readonly string? _storageConn;
//
//     public StorageQueue(IConfiguration configuration) => _storageConn = configuration.GetConnectionString("Storage");
//
//     public async Task SendMessageAsync(string queue, string message)
//     {
//         var queueClient = new QueueClient(_storageConn, queue);
//         await queueClient.SendMessageAsync(Convert.ToBase64String(Encoding.UTF8.GetBytes(message)));
//     }
//
//     public async Task CreateQueueAsync(string queue)
//     {
//         var queueClient = new QueueClient(_storageConn, queue);
//         await queueClient.CreateIfNotExistsAsync();
//     }
// }