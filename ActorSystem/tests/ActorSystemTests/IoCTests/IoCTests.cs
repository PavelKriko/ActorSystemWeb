using System.Collections.Concurrent;
using ActorSystem.Communication;
using ActorSystem.DI;

namespace ActorSystem.Tests;

public class IoCTests
{
    [Fact]
    public void IoCResolveReturnsIMailBox()
    {
        Assert.IsAssignableFrom<IMailBox>(IoC.Resolve<IMailBox>());
    }

    [Fact]
    public void IsTwoDifferentMailBox()
    {
        var mail1 = IoC.Resolve<IMailBox>();
        var mail2 = IoC.Resolve<IMailBox>();
        Assert.NotSame(mail1,mail2);
    }

    [Fact]
    public void IoCResolveReturnsMessageQueueSingleton()
    {
        var queue1 =  IoC.Resolve<IMessageQueue>();
        var queue2 =  IoC.Resolve<IMessageQueue>();
        Assert.IsAssignableFrom<IMessageQueue>(queue1);
        Assert.IsAssignableFrom<IMessageQueue>(queue2);
        Assert.Same(queue1,queue2);
    }
}