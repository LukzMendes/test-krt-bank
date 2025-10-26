using Krt.Bank.Domain.Common.Pagination;

namespace Krt.Bank.Application.Common.Pagination
{
    public static class PaginationDtoExtensions
    {
        public static PaginatedResponse<TDto> ToPaginatedResponse<TDto, TEntity>(this Paginated<TEntity> paginated,
            IEnumerable<TDto> items) where TDto : class, new() where TEntity : class
        {
            return new PaginatedResponse<TDto>
            {
                Page = paginated.Page,
                PageSize = paginated.PageSize,
                Total = paginated.Total,
                TotalPages = paginated.TotalPages,
                Items = items,
                OrderBy = paginated.OrderBy,
                Ascending = paginated.Ascending
            };
        }

        public static Paginate ToPaginated(this PaginatedRequest? request)
        {
            if (request == null)
            {
                return Paginate.Create(1, 10);
            }

            return Paginate.Create(request.Page, request.PageSize, request.Ascending, request.OrderBy);
        }
    }
}