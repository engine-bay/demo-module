namespace EngineBay.DemoModule.Endpoints
{
    public static class TodoItemEndpoints
    {
        private const string ItemBasePath = "/items";
        private const string ItemFullPath = "/lists/{listId:guid}/items";

        private static readonly string[] TodoItemTags = { ApiGroupNameConstants.TodoItem };

        public static void MapEndpoints(RouteGroupBuilder endpoints)
        {
            endpoints.MapPost(
                ItemFullPath,
                async (CreateTodoItem handler, CreateTodoItemDto createTodoItemDto, Guid listId, CancellationToken cancellation) =>
                {
                    var command = new CreateTodoItemCommand(createTodoItemDto.Name, listId)
                    {
                        Description = createTodoItemDto.Description,
                        DueDate = createTodoItemDto.DueDate,
                    };
                    var result = await handler.Handle(command, cancellation);
                    return Results.Created($"{TodoListEndpoints.ListBasePath}/{result.ListId}{ItemBasePath}/{result.Id}", result);
                })
                .WithTags(TodoItemTags);
        }
    }
}
