namespace EngineBay.DemoModule
{
    public static class DemoModuleEndpoints
    {
        private static readonly string[] TodoListTags = { ApiGroupNameConstants.TodoList };

        private static string listBasePath = "/todolist";
        private static string itemBasePath = "/todolist/{listId:guid}/item";

        public static void MapEndpoints(RouteGroupBuilder endpoints)
        {
            endpoints.MapPost(listBasePath, async (CreateTodoList handler, CreateTodoListDto createTodoListDto, CancellationToken cancellation) =>
            {
                var result = await handler.Handle(createTodoListDto, cancellation);
                return Results.Created($"{listBasePath}/{result.Id}", result);
            })
            .WithTags(TodoListTags);

            endpoints.MapGet(listBasePath, (Guid id, CancellationToken cancellation) =>
              {
                  return Results.Ok("todo list");
              })
              .WithTags(TodoListTags);

            endpoints.MapGet(listBasePath + "/{id}", async (GetTodoList handler, Guid id, CancellationToken cancellation) =>
              {
                  var result = await handler.Handle(id, cancellation);
                  return Results.Ok(result);
              })
              .WithTags(TodoListTags);

            endpoints.MapPost(itemBasePath, async (CreateTodoItem handler, CreateTodoItemDto createTodoItemDto, Guid listId, CancellationToken cancellation) =>
            {
                var command = new CreateTodoItemCommand(createTodoItemDto.Name, listId)
                {
                    Description = createTodoItemDto.Description,
                    DueDate = createTodoItemDto.DueDate,
                };
                var result = await handler.Handle(command, cancellation);
                return Results.Created($"{listBasePath}/{result.Id}", result);
            })
            .WithTags(TodoListTags);
        }
    }
}
