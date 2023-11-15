namespace EngineBay.DemoModule.Queries
{
    using EngineBay.Core;

    public class QueryTodoListRequest
    {
        public QueryTodoListRequest(PaginationParameters paginationParameters)
        {
            this.PaginationParameters = paginationParameters;
        }

        public PaginationParameters PaginationParameters { get; private set; }
    }
}
