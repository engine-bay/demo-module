namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateTodoItem : ICommandHandler<UpdateTodoItemCommand, TodoItemDto>
    {
        private readonly DemoModuleWriteDbContext demoModuleDb;
        private readonly IValidator<UpdateTodoItemCommand> validator;

        public UpdateTodoItem(
            DemoModuleWriteDbContext demoModuleDb,
            IValidator<UpdateTodoItemCommand> validator)
        {
            this.demoModuleDb = demoModuleDb;
            this.validator = validator;
        }

        public async Task<TodoItemDto> Handle(UpdateTodoItemCommand updateTodoItemCommand, CancellationToken cancellation)
        {
            if (updateTodoItemCommand is null)
            {
                throw new ArgumentNullException(nameof(updateTodoItemCommand));
            }

            this.validator.ValidateAndThrow(updateTodoItemCommand);

            var todoItem = await this.demoModuleDb.TodoItems.FindAsync(new object[] { updateTodoItemCommand.Id }, cancellation) ?? throw new NotFoundException($"No TodoItem with Id ${updateTodoItemCommand.Id} found.");
            todoItem.Name = updateTodoItemCommand.Name;
            todoItem.Completed = updateTodoItemCommand.Completed;
            todoItem.ListId = updateTodoItemCommand.ListId;
            todoItem.Description = updateTodoItemCommand.Description;
            todoItem.DueDate = updateTodoItemCommand.DueDate;

            await this.demoModuleDb.SaveChangesAsync(cancellation);

            return new TodoItemDto(todoItem);
        }
    }
}