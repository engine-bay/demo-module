namespace EngineBay.DemoModule
{
    using EngineBay.Core;

    public class QueryTodoItemRequest
    {
        public QueryTodoItemRequest(PaginationParameters paginationParameters)
        {
            this.PaginationParameters = paginationParameters;
        }

        public PaginationParameters PaginationParameters { get; private set; }
    }
}
