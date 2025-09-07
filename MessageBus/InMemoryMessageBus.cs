
using System.Collections.Concurrent;

namespace MessageBus;

public class InMemoryMessageBus : IMessageBus
{
    private readonly ConcurrentDictionary<string, Func<object, Task>> _subscriptions = new();

    public Task PublishAsync(object message)
    {
        foreach (var subscription in _subscriptions.Values)
        {
            subscription(message);
        }
        return Task.CompletedTask;
    }

    public Task SubscribeAsync(string subscriptionId, Func<object, Task> onMessage)
    {
        _subscriptions.TryAdd(subscriptionId, onMessage);
        return Task.CompletedTask;
    }
}
