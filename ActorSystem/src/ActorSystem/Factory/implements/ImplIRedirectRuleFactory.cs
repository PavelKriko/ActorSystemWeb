using ActorSystem.Communication;

namespace ActorSystem.Factory;

public class RedirectRuleFactory : IRedirectRuleFactory
{
    public IRedirectRuleRepository Create(IDictionary<string, IMailBox> mailBoxes, ISet<RedirectRule> rules)
    {
        return new RedirectRuleRepository(mailBoxes,rules);
    }
}