namespace EngineBay.DemoModule
{
    using EngineBay.Core;

    public class DemoModuleModule : BaseModule
    {
        public override IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
        {
            return services;
        }

        public override WebApplication AddMiddleware(WebApplication app)
        {
            return app;
        }
    }
}