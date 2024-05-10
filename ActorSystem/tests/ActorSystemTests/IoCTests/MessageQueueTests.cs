using ActorSystem.Communication;
using ActorSystem.DI;

namespace ActorSystem.Tests;

public class MessageQueueTests
{
    [Fact]
    public void QueueContainsMessage()
    {
        IMessageQueue queue = IoC.Resolve<IMessageQueue>();
        queue.Put(new Message());
        Assert.NotNull(queue.Take());
    }
}