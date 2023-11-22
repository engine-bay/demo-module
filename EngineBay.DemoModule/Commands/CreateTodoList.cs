namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateTodoList : ICommandHandler<CreateOrUpdateTodoListDto, TodoListDto>
    {
        private readonly DemoModuleWriteDbContext demoModuleDb;
        private readonly IValidator<CreateOrUpdateTodoListDto> validator;

        public CreateTodoList(
            DemoModuleWriteDbContext demoModuleDb,
            IValidator<CreateOrUpdateTodoListDto> validator)
        {
            this.demoModuleDb = demoModuleDb;
            this.validator = validator;
        }

        public async Task<TodoListDto> Handle(CreateOrUpdateTodoListDto createTodoListDto, CancellationToken cancellation)
        {
            ArgumentNullException.ThrowIfNull(createTodoListDto);

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