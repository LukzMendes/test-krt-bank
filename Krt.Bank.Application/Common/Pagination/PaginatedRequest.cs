namespace Krt.Bank.Application.Common.Pagination
{
    public class PaginatedRequest
    {
        public int? Page { get; set; } = 1;
        public int? PageSize { get; set; } = 10;
        public string[] OrderBy { get; set; } = ["createdAt"];
        public bool? Ascending { get; set; } = false;
    }
}
