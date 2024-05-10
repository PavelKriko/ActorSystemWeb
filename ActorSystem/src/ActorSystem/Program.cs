using ActorSystem;

using MongoDB.Driver;
using MongoDB.Bson;
using ActorSystem.Communication;
using ActorSystem.Actors;

var builder = WebApplication.CreateBuilder(args);

// Добавление контроллеров с поддержкой представлений
builder.Services.AddControllersWithViews();


// Настройка для разрешения синхронного ввода/вывода, если это необходимо
builder.Services.Configure<IISServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});

// Add services to the container.
var mongoDBSettings = builder.Configuration.GetSection("MongoDB").Get<MongoDBSettings>();

builder.Services.AddSingleton<IMongoClient>(sp =>
{ 
  return new MongoClient(mongoDBSettings!.ConnectionURI);
});
builder.Services.AddSingleton<IMongoCollection<BsonDocument>>(sp =>
{
  var client = sp.GetRequiredService<IMongoClient>();
  var database = client.GetDatabase(mongoDBSettings!.DatabaseName);
  return database.GetCollection<BsonDocument>(mongoDBSettings.CollectionName);
});

// Add the dependency from IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Включение поддержки статических файлов
app.UseStaticFiles();

app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

if (app.Environment.IsDevelopment())
{
    var client = new MongoClient(mongoDBSettings!.ConnectionURI); 
    var database = client.GetDatabase(mongoDBSettings.DatabaseName);
    var collection = database.GetCollection<BsonDocument>(mongoDBSettings.CollectionName);

    await collection.InsertOneAsync(BsonDocument.Parse("{ someField: 1 }"));

    // IMailBox mailBox = new MailBox();
    // IDictionary<SenderReceiverKey, IMailBox> rules = new Dictionary<SenderReceiverKey, IMailBox>();
    // IRedirectRuleRepository redirects = new RedirectRuleRepository(rules);
    // IMessageSystem messageSystem = new MessageSystem(redirects);
    // DBWriterActor actor = new DBWriterActor(messageSystem,mailBox,"123",collection,"key1", "key2", "key3");

    // var msg = new Message();
    // msg.Context["key1"] = 123;
    // msg.Context["key2"] = "ОПРСТ";
    // msg.Context["key3"] = true;

    // mailBox.PutMessage(msg);

    // await actor.HandleMessage();
}


app.Run();

public partial class Program { }