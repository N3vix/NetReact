using System.Text;
using System.Text.Json;
using NetReact.MessageBroker.SharedModels;

namespace NetReact.MessagingWorker;

public class MessagingWorkerService : BackgroundService
{
    private readonly IServiceScopeFactory _factory;
    private readonly MessageConsumer _consumer;

    public MessagingWorkerService(IServiceScopeFactory factory, MessageConsumer consumer)
    {
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(consumer);
        
        _factory = factory;
        _consumer = consumer;
    }
    
    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        _consumer.AddListener((_, args) =>
        {
            using var scope = _factory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<CodeChampDbContext>();
            var body = args.Body.ToArray();
            var postJson = Encoding.UTF8.GetString(body);
            var post = JsonSerializer.Deserialize<MessageAdded>(postJson)!;
            dbContext.Add(post);
            dbContext.SaveChanges();
        });
        
        return Task.CompletedTask;
    }
}