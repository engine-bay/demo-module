namespace EngineBay.DemoModule.Endpoints
{
    public static class TodoItemEndpoints
    {
        private const string ItemBasePath = "/lists/items";

        private static readonly string[] TodoItemTags = { ApiGroupNameConstants.TodoItem };

        public static void MapEndpoints(RouteGroupBuilder endpoints)
        {
            endpoints.MapPost(
                ItemBasePath,
                async (CreateTodoItem handler, CreateTodoItemDto createTodoItemDto, Guid listId, CancellationToken cancellation) =>
                {
                    var result = await handler.Handle(createTodoItemDto, cancellation);
                    return Results.Created($"{TodoListEndpoints.ListBasePath}/{result.Id}", result);
                })
                .WithTags(TodoItemTags);
        }
    }
}
