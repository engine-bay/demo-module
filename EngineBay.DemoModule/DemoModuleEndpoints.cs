namespace EngineBay.DemoModule
{
    using System.Security.Claims;
    using EngineBay.Core;

    public static class DemoModuleEndpoints
    {
        private static readonly string[] TodoListTags = { ApiGroupNameConstants.TodoList };

        private static string basePath = "/todolist";

        public static void MapEndpoints(RouteGroupBuilder endpoints)
        {
            endpoints.MapPost(basePath, async (CreateTodoList command, CreateTodoListDto createTodoListDto, CancellationToken cancellation) =>
            {
                var result = await command.Handle(createTodoListDto, cancellation);
                return Results.Created($"{basePath}/{result.Id}", result);
            })
            .WithTags(TodoListTags);

            endpoints.MapGet(basePath + "/{id}", (Guid id, CancellationToken cancellation) =>
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
        }
    }
}
