using ActorSystem.DI;
using ActorSystem.Factory;

namespace ActorSystem.Tests;

public class IMailBoxFactoryTest
{
    [Fact]
    public void CreateListIMailBoxesTest()
    {
        var mailBoxesFactory = IoC.Resolve<IDictMailBoxFactory>();
        HashSet<string> names = ["Почтовый ящик №1", "Почтовый ящик №2", "Почтовый ящик №3"];
        var mailBoxes = mailBoxesFactory.Create(names);
        Assert.NotSame(mailBoxes["Почтовый ящик №1"],mailBoxes["Почтовый ящик №2"]);
        Assert.NotSame(mailBoxes["Почтовый ящик №2"],mailBoxes["Почтовый ящик №3"]);
    }
}