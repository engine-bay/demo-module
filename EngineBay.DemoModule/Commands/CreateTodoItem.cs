namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateTodoItem : ICommandHandler<CreateTodoItemDto, TodoItemDto>
    {
        private readonly DemoModuleWriteDbContext demoModuleDb;
        private readonly IValidator<CreateTodoItemDto> validator;

        public CreateTodoItem(
            DemoModuleWriteDbContext demoModuleDb,
            IValidator<CreateTodoItemDto> validator)
        {
            this.demoModuleDb = demoModuleDb;
            this.validator = validator;
        }

        public async Task<TodoItemDto> Handle(CreateTodoItemDto createTodoItemDto, CancellationToken cancellation)
        {
            if (createTodoItemDto is null)
            {
                throw new ArgumentNullException(nameof(createTodoItemDto));
            }

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