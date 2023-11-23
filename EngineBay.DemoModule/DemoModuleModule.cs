namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using EngineBay.Persistence;
    using FluentValidation;
    using Microsoft.EntityFrameworkCore;

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
            return services;
        }

        public override RouteGroupBuilder MapEndpoints(RouteGroupBuilder endpoints)
        {
            TodoListEndpoints.MapEndpoints(endpoints);
            TodoItemEndpoints.MapEndpoints(endpoints);
            return endpoints;
        }

        public override WebApplication AddMiddleware(WebApplication app)
        {
            return app;
        }

        public IReadOnlyCollection<IModuleDbContext> GetRegisteredDbContexts(DbContextOptions<ModuleWriteDbContext> dbOptions)
        {
            return new IModuleDbContext[] { new DemoModuleDbContext(dbOptions) };
        }
    }
}