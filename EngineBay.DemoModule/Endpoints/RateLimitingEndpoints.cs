namespace EngineBay.DemoModule
{
    public static class RateLimitingEndpoints
    {
        private const string BasePath = "/rate-limit";

        private static readonly string[] RateLimitingItemTags = { ApiGroupNameConstants.RateLimiting };

        public static void MapEndpoints(RouteGroupBuilder endpoints)
        {
            endpoints.MapGet(
                BasePath + "/fixed/{id}",
                async (HttpContext context, string id, CancellationToken cancellation) =>
                {
                    return await Task.Run(() => Results.Ok($"success from {id}"));
                })
                .WithTags(RateLimitingItemTags)
                .RequireRateLimiting("fixed");

            endpoints.MapGet(
                BasePath + "/fixed2/{id}",
                async (HttpContext context, string id, CancellationToken cancellation) =>
                {
                    return await Task.Run(() => Results.Ok($"success from {id}"));
                })
                .WithTags(RateLimitingItemTags)
                .RequireRateLimiting("fixed");

            endpoints.MapGet(
                BasePath + "/sliding/{id}",
                async (HttpContext context, string id, CancellationToken cancellation) =>
                {
                    return await Task.Run(() => Results.Ok($"success from {id}"));
                })
                .WithTags(RateLimitingItemTags)
                .RequireRateLimiting("sliding");

            endpoints.MapGet(
                BasePath + "/token/{id}",
                async (HttpContext context, string id, CancellationToken cancellation) =>
                {
                    return await Task.Run(() => Results.Ok($"success from {id}"));
                })
                .WithTags(RateLimitingItemTags)
                .RequireRateLimiting("token");

            endpoints.MapGet(
            BasePath + "/concurrency/{id}",
            async (HttpContext context, string id, CancellationToken cancellation) =>
            {
                await Task.Delay(500, cancellation);
                return await Task.Run(() => Results.Ok($"success from {id}"));
            })
            .WithTags(RateLimitingItemTags)
            .RequireRateLimiting("concurrency");
        }
    }
}
