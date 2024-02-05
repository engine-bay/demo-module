namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using EngineBay.Telemetry;
    using FluentValidation;

    public class CreateTodoList : ICommandHandler<CreateOrUpdateTodoListDto, TodoListDto>
    {
        private readonly DemoModuleWriteDbContext demoModuleDb;
        private readonly IValidator<CreateOrUpdateTodoListDto> validator;
        private readonly ILogger<CreateTodoList> logger;

        public CreateTodoList(
            DemoModuleWriteDbContext demoModuleDb,
            IValidator<CreateOrUpdateTodoListDto> validator,
            ILogger<CreateTodoList> logger)
        {
            this.demoModuleDb = demoModuleDb;
            this.validator = validator;
            this.logger = logger;
        }

        public async Task<TodoListDto> Handle(CreateOrUpdateTodoListDto createTodoListDto, CancellationToken cancellation)
        {
            using var activity = EngineBayActivitySource.Source.StartActivity(TracingActivityNameConstants.Handler + DemoActivityNameConstants.TodoListCreate);

            ArgumentNullException.ThrowIfNull(createTodoListDto);

            this.validator.ValidateAndThrow(createTodoListDto);

            var todoList = new TodoList(createTodoListDto.Name)
            {
                Description = createTodoListDto.Description,
            };

            this.logger.NewTodoList(todoList.Id);

            await this.demoModuleDb.TodoLists.AddAsync(todoList, cancellation);
            await this.demoModuleDb.SaveChangesAsync(cancellation);

            return new TodoListDto(todoList);
        }
    }
}