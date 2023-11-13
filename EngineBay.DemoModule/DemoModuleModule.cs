namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class DemoModuleModule : BaseModule
    {
        public override IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
        {
            // Register commands
            services.AddTransient<CreateTodoList>();
            services.AddTransient<UpdateTodoList>();
            services.AddTransient<CreateTodoItem>();

            // Register queries
            services.AddTransient<GetTodoList>();
            services.AddTransient<QueryTodoList>();

            // Register validators
            services.AddTransient<IValidator<CreateTodoListDto>, CreateTodoListDtoValidator>();
            services.AddTransient<IValidator<UpdateTodoListCommand>, UpdateTodoListDtoValidator>();
            services.AddTransient<IValidator<CreateTodoItemCommand>, CreateTodoItemDtoValidator>();

            // register persistence services
            var databaseConfiguration = new CQRSDatabaseConfiguration<DemoModuleDbContext, DemoModuleQueryDbContext, DemoModuleWriteDbContext>();
            databaseConfiguration.RegisterDatabases(services);
            return services;
        }

        public override RouteGroupBuilder MapEndpoints(RouteGroupBuilder endpoints)
        {
            DemoModuleEndpoints.MapEndpoints(endpoints);
            return endpoints;
        }

        public override WebApplication AddMiddleware(WebApplication app)
        {
            return app;
        }
    }
}