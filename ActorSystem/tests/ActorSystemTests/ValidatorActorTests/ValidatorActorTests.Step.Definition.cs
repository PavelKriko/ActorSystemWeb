using ActorSystem.Communication;
using ActorSystem.Actors;
using TechTalk.SpecFlow;

namespace ActorSystem.Tests;

[Binding]
public class ValidatorTests
{
    IMailBox? validatorMailBox;
    IMailBox? trueValidationMailBox;
    IMailBox? falseValidationMailBox;
    IRedirectRuleRepository? ruleRepo;
    IMessageSystem? messageSystem;
    Validator<int>? validator;

    [Given("Система состоящая из 3 почтовых ящиков")]
    public void threeBoxesSystem()
    {
        validatorMailBox = new MailBox();
        trueValidationMailBox = new MailBox();
        falseValidationMailBox = new MailBox();

        //Почтовые ящики
        var mailBoxes = new Dictionary<string, IMailBox> {
            ["Актор№1"] = validatorMailBox,
            ["Актор№2"] = trueValidationMailBox,
            ["Актор№3"] = falseValidationMailBox,
        };

        //Правила перессылки сообщений
        var rules = new HashSet<RedirectRule>{
            new RedirectRule("Актор№1","ValidationTrue","Актор№2"),
            new RedirectRule("Актор№1","ValidationFalse","Актор№3")
        };

        ruleRepo = new RedirectRuleRepository(mailBoxes, rules);
        messageSystem = new MessageSystem(ruleRepo);

        validator = new Validator<int>(messageSystem,validatorMailBox, "Актор№1", "age", 18);

    }

    [When("В валидотор приходит сообщение со значением, которое нужно проверить, оно не проходит валидацию")]
    public async Task ValidatorGetFalseValidationMessage()
    {
        var msg = new Message();
        msg.Context["age"] = 14;
        validatorMailBox!.PutMessage(msg);
        await validator!.HandleMessage();
    }

    [Then("В ящике FalseValidation появляется сообщение")]
    public async Task falseValidationMailBoxGetMessage()
    {
        Assert.NotNull(await falseValidationMailBox!.GetMessage());
    }
}