namespace EngineBay.DemoModule
{
    public static class DemoModuleEndpoints
    {
        private static readonly string[] TodoListTags = { ApiGroupNameConstants.TodoList };

        private static string listBasePath = "/todolist";
        private static string itemBasePath = "/todolist/{listId:guid}/item";

        public static void MapEndpoints(RouteGroupBuilder endpoints)
        {
            endpoints.MapPost(listBasePath, async (CreateTodoList command, CreateTodoListDto createTodoListDto, CancellationToken cancellation) =>
            {
                var result = await command.Handle(createTodoListDto, cancellation);
                return Results.Created($"{listBasePath}/{result.Id}", result);
            })
            .WithTags(TodoListTags);

            endpoints.MapGet(listBasePath + "/{id}", (Guid id, CancellationToken cancellation) =>
              {
                  return Results.Ok("todo list");
              })
              .WithTags(TodoListTags);

            // endpoints.MapGet(
            //     "/todolist",
            //     async (int? skip, int? limit, string? sortBy, SortOrderType? sortOrder, string? actionType, DateTime? startDate, DateTime? endDate, ClaimsPrincipal claimsPrincipal, CancellationToken cancellation) =>
            //     {
            //         return Results.Ok("todo list");
            //     })
            //   .RequireAuthorization(ModulePolicies.AdminAuditEntries)
            //   .WithTags(TodoList);
            endpoints.MapPost(itemBasePath, async (CreateTodoItem command, CreateTodoItemDto createTodoItemDto, Guid listId, CancellationToken cancellation) =>
            {
                createTodoItemDto.ListId = listId;
                var result = await command.Handle(createTodoItemDto, cancellation);
                return Results.Created($"{listBasePath}/{result.Id}", result);
            })
            .WithTags(TodoListTags);
        }
    }
}
