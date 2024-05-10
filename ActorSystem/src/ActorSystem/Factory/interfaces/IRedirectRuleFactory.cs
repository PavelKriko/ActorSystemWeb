using ActorSystem.Communication;

namespace ActorSystem.Factory;

public interface IRedirectRuleFactory
{
    IRedirectRuleRepository Create(IDictionary<string, IMailBox> mailBoxes, ISet<RedirectRule> rules);
}