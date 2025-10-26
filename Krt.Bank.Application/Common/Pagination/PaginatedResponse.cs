namespace Krt.Bank.Application.Common.Pagination
{
    public class PaginatedResponse<T> where T : class
    {
        public int? Page { get; set; }
        public int? PageSize { get; set; }
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public string[] OrderBy { get; set; }
        public bool? Ascending { get; set; }

        public IEnumerable<T> Items { get; set; } = new List<T>();
    }
}
