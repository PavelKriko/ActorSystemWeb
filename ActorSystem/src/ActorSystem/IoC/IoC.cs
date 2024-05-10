using ActorSystem.Communication;
using ActorSystem.Factory;

namespace ActorSystem.DI;

public static class IoC
    {
        private static readonly IServiceProvider _provider;

        static IoC()
        {
            var services = new ServiceCollection();

            services.AddTransient<IMailBox, MailBox>();
            services.AddSingleton<IMessageQueue, MessageQueue>();
            services.AddSingleton<IDictMailBoxFactory, DictMailBoxFactory>();
            services.AddSingleton<IRedirectRuleFactory,RedirectRuleFactory>();

            _provider = services.BuildServiceProvider();
        }

    public static T Resolve<T>() where T : class
    {
        return _provider.GetRequiredService<T>();
    }
}
