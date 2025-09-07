
namespace MessageBus;

public interface IMessageBus
{
    Task PublishAsync(object message);
    Task SubscribeAsync(string subscriptionId, Func<object, Task> onMessage);
}
