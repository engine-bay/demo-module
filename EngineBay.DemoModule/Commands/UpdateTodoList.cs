namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using FluentValidation;

    public class UpdateTodoList : ICommandHandler<UpdateTodoListCommand, TodoListDto>
    {
        private readonly DemoModuleWriteDbContext demoModuleDb;
        private readonly IValidator<UpdateTodoListCommand> validator;

        public UpdateTodoList(
            DemoModuleWriteDbContext demoModuleDb,
            IValidator<UpdateTodoListCommand> validator)
        {
            this.demoModuleDb = demoModuleDb;
            this.validator = validator;
        }

        public async Task<TodoListDto> Handle(UpdateTodoListCommand updateTodoListCommand, CancellationToken cancellation)
        {
            if (updateTodoListCommand is null)
            {
                throw new ArgumentNullException(nameof(updateTodoListCommand));
            }

            this.validator.ValidateAndThrow(updateTodoListCommand);

            var todoList = await this.demoModuleDb.TodoLists.FindAsync(new object[] { updateTodoListCommand.Id }, cancellation) ?? throw new NotFoundException($"No TodoList with Id ${updateTodoListCommand.Id} found.");
            todoList.Name = updateTodoListCommand.Name;
            todoList.Description = updateTodoListCommand.Description;

            await this.demoModuleDb.SaveChangesAsync(cancellation);

            return new TodoListDto(todoList);
        }
    }
}