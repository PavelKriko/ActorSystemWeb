using ActorSystem.Communication;

namespace ActorSystem.Actors;

public class Validator<T> : ActorBase where T : IComparable<T>
{
    private T _compareValue;
    private string _key;
    public Validator(IMessageSystem messageSystem, IMailBox mailBox,string ID,string key, T compareValue) : base(messageSystem, mailBox, ID)
    {
        _compareValue = compareValue;
        _key = key;
    }

    public override async Task HandleMessage()
    {
        var message = await _mailbox.GetMessage();
        object? valueObject;
        if(message.Context.TryGetValue(_key, out valueObject))
        {
            if(valueObject is T)
            {
                var msg = new Message();
                msg.Sender = _id;
                msg.Context = message.Context;

                //Если valueObject < _compareValue
                if(((T)valueObject).CompareTo(_compareValue) < 0)
                {
                    msg.Receiver = "ValidationFalse";
                }
                else
                {
                    msg.Receiver = "ValidationTrue";
                }
                
                _messageSystem.requestMessage(msg);
            }
        }
        
    }
}