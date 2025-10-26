namespace Krt.Bank.Domain.Common.Pagination
{
    public class Paginated<T> where T : class
    {
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public int Total { get; set; }
        public int TotalPages { get; set; }
        public string[] OrderBy { get; set; }
        public bool? Ascending { get; set; }

        public List<T> Items { get; set; } = new List<T>();

        public Paginated(int? page, int? pageSize, int total, IEnumerable<T> items, string[]? orderBy = null, bool? ascending = null)
        {
            Page = page;
            PageSize = pageSize;
            Total = total;
            TotalPages = (int)Math.Ceiling(total / (double)pageSize);
            Items = items.ToList();
            OrderBy = orderBy;
            Ascending = ascending;
        }
    }
}
