using Microsoft.AspNetCore.Mvc;
using ActorSystem.Communication;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View(new MessageViewModel());
    }

    [HttpPost]
    public IActionResult SendMessage(MessageViewModel model)
    {
        if (ModelState.IsValid)
        {
            var message = new Message
            {
                Receiver = "ReceiverName", // Пример, задайте реальное значение
                Sender = "SenderName", // Пример, задайте реальное значение
                Context = new Dictionary<string, object?>
                {
                    {"Group", model.Group},
                    {"Name", model.Name},
                    {"Description", model.Description},
                    {"Files", model.Files} // Просто сохраняем список файлов, не загружая их
                }
            };

            // Отправка сообщения в акторную систему (псевдокод)
            // ActorSystem.Send(message);

            return RedirectToAction("Index");
        }

        return View("Index", model);
    }
}
