using ActorSystem.Communication;
using ActorSystem.DI;
using ActorSystem.Factory;

namespace ActorSystem.Tests;

public class IRedirectRuleFactoryTests
{
    [Fact]
    public void IRedirectRuleFactoryContainsRedirect()
    {
        var redirectRuleFactory = IoC.Resolve<IRedirectRuleFactory>();
        Assert.NotNull(redirectRuleFactory);
    }

    [Fact]
    public void IRedirectRuleFactoryReturnsCorrectRedirects()
    {   
        var names = new HashSet<string> {"Актор№1","Актор№2","Актор№3"};

        var mailBoxes = IoC.Resolve<IDictMailBoxFactory>().Create(names);
        //№1 -> №2 -> №3
        var rules = new HashSet<RedirectRule>{
            new RedirectRule("Актор№1", "ПравилоДляАктора№1","Актор№2"),
            new RedirectRule("Актор№2", "ПравилоДляАктора№2","Актор№3"),
        };

        var redirectRuleFactory = IoC.Resolve<IRedirectRuleFactory>();
        var RedirectRuleRepository = redirectRuleFactory.Create(mailBoxes,rules);
    }
}