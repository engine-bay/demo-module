namespace EngineBay.DemoModule
{
    using EngineBay.Core;
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
            ArgumentNullException.ThrowIfNull(createTodoListDto);

            this.logger.NewTodoList(createTodoListDto.Name);

            this.validator.ValidateAndThrow(createTodoListDto);

            var todoList = new TodoList(createTodoListDto.Name)
            {
                Description = createTodoListDto.Description,
            };

            await this.demoModuleDb.TodoLists.AddAsync(todoList, cancellation);
            await this.demoModuleDb.SaveChangesAsync(cancellation);

            return new TodoListDto(todoList);
        }
    }
}