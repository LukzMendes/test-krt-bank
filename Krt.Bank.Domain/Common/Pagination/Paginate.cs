namespace Krt.Bank.Domain.Common.Pagination
{
    public class Paginate
    {
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public List<String> OrderBy { get; set; } = new();
        public bool? Ascending { get; set; } = true;

        private Paginate(int page, int pageSize, bool? ascending, string[]? orderBy)
        {
            Page = page;
            PageSize = pageSize;
            Ascending = ascending ?? true;
            if (orderBy != null) OrderBy = orderBy.ToList();
        }

        public static Paginate Create(int? page, int? pageSize, bool? ascending = true, params string[]? orderBy)
        {
            return new Paginate(page ?? 1, pageSize ?? 10, ascending, orderBy);
        }

    }
}
