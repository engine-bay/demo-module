namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateTodoItem : ICommandHandler<CreateTodoItemCommand, TodoItemDto>
    {
        private readonly DemoModuleWriteDbContext demoModuleDb;
        private readonly IValidator<CreateTodoItemCommand> validator;

        public CreateTodoItem(
            DemoModuleWriteDbContext demoModuleDb,
            IValidator<CreateTodoItemCommand> validator)
        {
            this.demoModuleDb = demoModuleDb;
            this.validator = validator;
        }

        public async Task<TodoItemDto> Handle(CreateTodoItemCommand createTodoItemDto, CancellationToken cancellation)
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

            this.demoModuleDb.TodoItems.Add(todoItem);
            await this.demoModuleDb.SaveChangesAsync(cancellation);

            return new TodoItemDto(todoItem);
        }
    }
}