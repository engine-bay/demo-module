namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using EngineBay.Telemetry;
    using FluentValidation;

    public class UpdateTodoList : ICommandHandler<UpdateTodoListCommand, TodoListDto>
    {
        private readonly DemoModuleWriteDbContext demoModuleDb;
        private readonly IValidator<UpdateTodoListCommand> validator;
        private readonly ILogger<UpdateTodoList> logger;

        public UpdateTodoList(
            DemoModuleWriteDbContext demoModuleDb,
            IValidator<UpdateTodoListCommand> validator,
            ILogger<UpdateTodoList> logger)
        {
            this.demoModuleDb = demoModuleDb;
            this.validator = validator;
            this.logger = logger;
        }

        public async Task<TodoListDto> Handle(UpdateTodoListCommand updateTodoListCommand, CancellationToken cancellation)
        {
            using var activity = EngineBayActivitySource.Source.StartActivity(TracingActivityNameConstants.Handler + DemoActivityNameConstants.TodoListUpdate);

            ArgumentNullException.ThrowIfNull(updateTodoListCommand);

            this.logger.UpdateTodoList(updateTodoListCommand.Id);

            this.validator.ValidateAndThrow(updateTodoListCommand);

            var todoList = await this.demoModuleDb.TodoLists.FindAsync(new object[] { updateTodoListCommand.Id }, cancellation) ?? throw new NotFoundException($"No TodoList with Id ${updateTodoListCommand.Id} found.");
            todoList.Name = updateTodoListCommand.Name;
            todoList.Description = updateTodoListCommand.Description;

            await this.demoModuleDb.SaveChangesAsync(cancellation);

            return new TodoListDto(todoList);
        }
    }
}