using System.Collections.Concurrent;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;
using Microsoft.Extensions.Configuration;

namespace ActorSystem.Communication;

public class EventLoop
{   
    IRedirectRuleRepository Repository;
    IMessageQueue Queue;
    Thread thread;
    public EventLoop(IRedirectRuleRepository repository, IMessageQueue queue)
    {
        Repository = repository;
        Queue = queue;
        thread = new Thread(loopQueue);
        thread.Start();
    }

    public void Wait()
    {
        thread.Join();
    }

    private void loopQueue()
    {
        while(true)
        {
            var message = Queue.Take();
            if(message.Context.ContainsKey("STOP_EVENT_LOOP"))
            {
                break;
            }
            try
            {
                var key = new SenderReceiverKey(message.Sender, message.Receiver);
                var endAddres = Repository.GetAdressReceiver(key);
                endAddres!.PutMessage(message);
            }
            catch
            {
                throw;
            }
        }
    }
}