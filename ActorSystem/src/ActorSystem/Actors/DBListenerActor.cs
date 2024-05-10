using ActorSystem.Communication;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ActorSystem.Actors;
public class DBListenerActor : ActorBase
{
    string[] _keys;
    IMongoCollection<BsonDocument> _collection;
    public DBListenerActor(IMessageSystem messageSystem, IMailBox mailBox,string ID,IMongoCollection<BsonDocument> collection, params string[] keys) : base(messageSystem, mailBox, ID)
    {
        _collection = collection;
        _keys = keys;
    }

    public override async Task HandleMessage()
    {
        var message = await _mailbox.GetMessage();
        var document = new BsonDocument();
        foreach(var key in _keys)
        {
            if(message.Context.ContainsKey(key))
            {
                if (message.Context[key] is IFormFile file)
                {
                    using (var ms = new MemoryStream())
                    {
                        await file.CopyToAsync(ms);
                        document[key] = ms.ToArray();
                        document[$"{key}_FileName"] = file.FileName; // Сохраняем имя файла отдельно
                        document[$"{key}_ContentType"] = file.ContentType; // Сохраняем тип содержимого файла
                    }
                }
                else
                {
                    document[key] = BsonValue.Create(message.Context[key]);
                }
            }
        }
        _collection.InsertOne(document);
    }
}