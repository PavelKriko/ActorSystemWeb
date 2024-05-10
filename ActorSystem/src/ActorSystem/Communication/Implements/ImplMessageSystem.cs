namespace ActorSystem.Communication;

public class MessageSystem(IRedirectRuleRepository redirectRuleRepository) : IMessageSystem
{
    public void requestMessage(IMessage message)
    {
        var endAddres = redirectRuleRepository.GetAdressReceiver(new SenderReceiverKey(message.Sender, message.Receiver));
        endAddres!.PutMessage(message);
    }
}