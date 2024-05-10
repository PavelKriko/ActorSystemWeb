using ActorSystem.Communication;
using TechTalk.SpecFlow;

namespace ActorSystem.Test;

[Binding]
public class LocalProcessingMessagesTest
{   
    IRedirectRuleRepository? ruleRepo;
    IMessageSystem? messageSystem;
    IMailBox? MailBox1;
    IMailBox? MailBox2;


    [Given("Система состоящая их почтовых ящиков")]
    public void Address_For_MailBoxes()
    {   
        MailBox1 = new MailBox();
        MailBox2 = new MailBox();
        //Почтовые ящики
        var mailBoxes = new Dictionary<string, IMailBox> {
            ["Актор№1"] = MailBox1,
            ["Актор№2"] = MailBox2,
        };

        //Правила перессылки сообщений
        var rules = new HashSet<RedirectRule>{
            new RedirectRule("Актор№1","Актор№2","Актор№2"),
            new RedirectRule("Актор№2","Актор№1","Актор№1")
        };
        ruleRepo = new RedirectRuleRepository(mailBoxes, rules);
        messageSystem = new MessageSystem(ruleRepo);
    }

    [When("Из ящика№1 отправляется сообщение в ящик№2")]
    public void Send_To_Mail2()
    {   
        IMessage msg = new Message();
        msg.Sender = "Актор№1";
        msg.Receiver = "Актор№2";
        messageSystem!.requestMessage(msg);
    }
    [Then("В ящике№2 появляется сообщение")]
    public async void Mail2_Contains_messages(){
        Assert.NotNull(await MailBox2!.GetMessage());
    }
}