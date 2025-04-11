namespace ChatMessagesApp.Core.Application.Responses
{
    public class PaginatedResult<T>(List<T> items, int totalItems, int pageNumber, int pageSize)
    {
        public List<T> Items { get; } = items;
        public int TotalItems { get; } = totalItems;
        public int PageNumber { get; } = pageNumber;
        public int PageSize { get; } = pageSize;
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public int TotalUnseenNotification { get; set; } = 0;
    }
}
