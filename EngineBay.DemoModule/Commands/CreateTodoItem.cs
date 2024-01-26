namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateTodoItem : ICommandHandler<CreateTodoItemDto, TodoItemDto>
    {
        private readonly DemoModuleWriteDbContext demoModuleDb;
        private readonly IValidator<CreateTodoItemDto> validator;
        private readonly ILogger<CreateTodoItem> logger;

        public CreateTodoItem(
            DemoModuleWriteDbContext demoModuleDb,
            IValidator<CreateTodoItemDto> validator,
            ILogger<CreateTodoItem> logger)
        {
            this.demoModuleDb = demoModuleDb;
            this.validator = validator;
            this.logger = logger;
        }

        public async Task<TodoItemDto> Handle(CreateTodoItemDto createTodoItemDto, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(createTodoItemDto);

            this.logger.NewTodoItem(createTodoItemDto.Name);

            this.validator.ValidateAndThrow(createTodoItemDto);

            var todoItem = new TodoItem(createTodoItemDto.Name, createTodoItemDto.ListId, false)
            {
                Description = createTodoItemDto.Description,
                DueDate = createTodoItemDto.DueDate,
            };

            await this.demoModuleDb.TodoItems.AddAsync(todoItem, cancellation);
            await this.demoModuleDb.SaveChangesAsync(cancellation);

            return new TodoItemDto(todoItem);
        }
    }
}