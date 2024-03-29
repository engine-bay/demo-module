namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;

    public class DemoModuleModule : BaseModule, IDatabaseModule
    {
        public override IServiceCollection RegisterModule(IServiceCollection services, IConfiguration configuration)
        {
            // Register commands
            services.AddTransient<CreateTodoList>();
            services.AddTransient<UpdateTodoList>();
            services.AddTransient<DeleteTodoList>();
            services.AddTransient<CreateTodoItem>();
            services.AddTransient<UpdateTodoItem>();
            services.AddTransient<DeleteTodoItem>();

            // Register queries
            services.AddTransient<QueryTodoList>();
            services.AddTransient<GetTodoList>();
            services.AddTransient<QueryTodoItem>();
            services.AddTransient<GetTodoItem>();

            // Register validators
            services.AddTransient<IValidator<CreateOrUpdateTodoListDto>, CreateTodoListDtoValidator>();
            services.AddTransient<IValidator<UpdateTodoListCommand>, UpdateTodoListDtoValidator>();
            services.AddTransient<IValidator<CreateTodoItemDto>, CreateTodoItemDtoValidator>();
            services.AddTransient<IValidator<UpdateTodoItemCommand>, UpdateTodoItemDtoValidator>();

            // register persistence services
            var databaseConfiguration = new CQRSDatabaseConfiguration<DemoModuleDbContext, DemoModuleQueryDbContext, DemoModuleWriteDbContext>();
            databaseConfiguration.RegisterDatabases(services);

            // Register metrics
            // services.AddSingleton<MetricsDefinitions>();
            return services;
        }

        public override RouteGroupBuilder MapEndpoints(RouteGroupBuilder endpoints)
        {
            TodoListEndpoints.MapEndpoints(endpoints);
            TodoItemEndpoints.MapEndpoints(endpoints);
            RateLimitingEndpoints.MapEndpoints(endpoints);
            return endpoints;
        }

        public override WebApplication AddMiddleware(WebApplication app)
        {
            return app;
        }

        public IReadOnlyCollection<IModuleDbContext> GetRegisteredDbContexts(IDbContextOptionsFactory dbContextOptionsFactory)
        {
            ArgumentNullException.ThrowIfNull(dbContextOptionsFactory);

            return new IModuleDbContext[] { new DemoModuleDbContext(dbContextOptionsFactory.GetDbContextOptions<ModuleWriteDbContext>()) };
        }
    }
}