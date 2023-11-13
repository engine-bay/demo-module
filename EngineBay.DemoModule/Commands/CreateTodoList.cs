namespace EngineBay.DemoModule
{
    using EngineBay.Core;
    using FluentValidation;

    public class CreateTodoList : ICommandHandler<CreateTodoListDto, TodoListDto>
    {
        private readonly DemoModuleWriteDbContext demoModuleDb;
        private readonly IValidator<CreateTodoListDto> validator;

        public CreateTodoList(
            DemoModuleWriteDbContext demoModuleDb,
            IValidator<CreateTodoListDto> validator)
        {
            this.demoModuleDb = demoModuleDb;
            this.validator = validator;
        }

        public async Task<TodoListDto> Handle(CreateTodoListDto createTodoListDto, CancellationToken cancellation)
        {
            if (createTodoListDto is null)
            {
                throw new ArgumentNullException(nameof(createTodoListDto));
            }

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