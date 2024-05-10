namespace ActorSystem.Communication;

public class RedirectRuleRepository : IRedirectRuleRepository
{
    IDictionary<SenderReceiverKey, IMailBox> _endPointReceivers;
    public RedirectRuleRepository(IDictionary<string,IMailBox> mailBoxes, ISet<RedirectRule> redirectRules)
    {
        _endPointReceivers = new Dictionary<SenderReceiverKey, IMailBox>();
        foreach(var rule in redirectRules)
        {
            if(mailBoxes.ContainsKey(rule.Sender) && mailBoxes.ContainsKey(rule.Receiver))
            {
                _endPointReceivers[new SenderReceiverKey(rule.Sender, rule.Rule)] = mailBoxes[rule.Receiver];
            }
            else
            {
                throw new ArgumentException(string.Format("Именованные почтовые ящики не содержат одного/двух адресов {0}, {1}", rule.Receiver, rule.Sender));
            }

        }
    }
    public IMailBox GetAdressReceiver(SenderReceiverKey key)
    {
        try
        {
            return _endPointReceivers[key];
        }
        catch(KeyNotFoundException)
        {
            throw new KeyNotFoundException(string.Format("Не удалось найти получателя сообщения {0}, отправленного от {1}", key.Receiver, key.Sender));
        }
    }
}