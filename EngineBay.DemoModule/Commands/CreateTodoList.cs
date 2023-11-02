namespace EngineBay.DemoModule
{
    using System.Security.Claims;
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

            this.demoModuleDb.TodoLists.Add(todoList);
            await this.demoModuleDb.SaveChangesAsync(cancellation);

            return new TodoListDto(todoList);
        }
    }
}