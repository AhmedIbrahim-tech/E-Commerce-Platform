namespace Application.Wrappers
{
    public class PaginatedResult<T>
    {
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int TotalCount { get; set; } = 0;
        public object Meta { get; set; } = new();
        public int PageSize { get; set; } = 10;
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;
        public List<string> Messages { get; set; } = [];
        public bool Succeeded { get; set; } = false;
        public List<T> Data { get; set; } = [];

        public PaginatedResult(List<T> data)
        {
            Data = data;
        }

        internal PaginatedResult(bool succeeded, List<T> data = default, List<string> messages = null, int count = 0, int page = 1, int pageSize = 10)
        {
            Data = data;
            CurrentPage = page;
            Succeeded = succeeded;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }

        public static PaginatedResult<T> Success(List<T> data, int count, int page, int pageSize)
        {
            return new(true, data, null, count, page, pageSize);
        }
    }
}
