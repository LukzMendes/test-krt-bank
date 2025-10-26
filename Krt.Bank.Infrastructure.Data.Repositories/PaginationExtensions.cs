using Krt.Bank.Domain.Common.Pagination;
using Microsoft.EntityFrameworkCore;

namespace Krt.Bank.Infrastructure.Data.Repositories
{
    public static class PaginationExtensions
    {
        public static Paginated<T> Paginate<T>(this IQueryable<T> query, Paginate? paginate) where T : class
        {
            if (paginate != null && paginate.OrderBy != null)
            {
                paginate.OrderBy.ForEach(x =>
                {
                    var column = char.ToUpper(x[0]) + x.Substring(1);

                    if (query.FirstOrDefault()?.GetType().GetProperty(column) != null)
                    {
                        if (paginate.Ascending!.Value)
                        {
                            query = query.OrderBy(x => EF.Property<object>(x, column));
                        }
                        else
                        {
                            query = query.OrderByDescending(x => EF.Property<object>(x, column));
                        }
                    }
                });
            }

            var total = query.Count();

            if (paginate == null)
            {
                return new Paginated<T>(1, total, total, query.ToList());
            }

            var data = query.Skip((paginate!.Page - 1) * paginate.PageSize).Take(paginate.PageSize).ToList();

            return new Paginated<T>(paginate.Page, paginate.PageSize, total, data, paginate?.OrderBy?.ToArray(),
                paginate?.Ascending);
        }


        public static async Task<Paginated<T>> PaginateAsync<T>(this IQueryable<T> query, Paginate? paginate)
            where T : class
        {
            if (paginate != null && paginate.OrderBy != null)
            {
                paginate.OrderBy.ForEach(x =>
                {
                    var column = char.ToUpper(x[0]) + x.Substring(1);

                    if (query.FirstOrDefault()?.GetType().GetProperty(column) != null)
                    {
                        if (paginate.Ascending!.Value)
                        {
                            query = query.OrderBy(x => EF.Property<object>(x, column));
                        }
                        else
                        {
                            query = query.OrderByDescending(x => EF.Property<object>(x, column));
                        }
                    }
                });
            }

            var total = await query.CountAsync();

            if (paginate == null)
            {
                return new Paginated<T>(1, total, total, await query.ToListAsync());
            }

            var data = await query.Skip((paginate.Page - 1) * paginate.PageSize).Take(paginate.PageSize).ToListAsync();

            return new Paginated<T>(paginate.Page, paginate.PageSize, total, data, paginate?.OrderBy?.ToArray(),
                paginate?.Ascending);
        }
        public static Paginated<T> PaginateInMemory<T>(this IEnumerable<T> source, Paginate? paginate) where T : class
        {
            if (paginate != null && paginate.OrderBy != null)
            {
                foreach (var orderBy in paginate.OrderBy)
                {
                    var property = typeof(T).GetProperty(orderBy, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

                    if (property != null)
                    {
                        source = paginate.Ascending.GetValueOrDefault()
                            ? source.OrderBy(x => property.GetValue(x, null))
                            : source.OrderByDescending(x => property.GetValue(x, null));
                    }
                }
            }

            var total = source.Count();

            if (paginate == null)
            {
                return new Paginated<T>(1, total, total, source.ToList());
            }

            var data = source
                .Skip((paginate.Page - 1) * paginate.PageSize)
                .Take(paginate.PageSize)
                .ToList();

            return new Paginated<T>(paginate.Page, paginate.PageSize, total, data, paginate.OrderBy?.ToArray(), paginate.Ascending);
        }
    }
}
