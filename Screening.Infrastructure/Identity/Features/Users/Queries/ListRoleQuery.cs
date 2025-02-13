using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Screening.Common.Extensions;
using Screening.Common.Wrapper;

namespace Screening.Infrastructure.Identity.Features.Users.Queries
{
    public class ListRoleQuery : IRequest<ResponseWrapper<PagedList<IdentityRole<int>>>>
    {
        public DataGridQuery GridQuery { get; set; }
    }

    public class ListRoleQueryHandler : IRequestHandler<ListRoleQuery, ResponseWrapper<PagedList<IdentityRole<int>>>>
    {
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public ListRoleQueryHandler(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<ResponseWrapper<PagedList<IdentityRole<int>>>> Handle(ListRoleQuery request, CancellationToken cancellationToken)
        {
            var query = _roleManager.Roles.AsQueryable();

            // Apply search filter if search term is provided
            if (!string.IsNullOrEmpty(request.GridQuery.Search))
            {
                query = query.Where(r => r.Name.Contains(request.GridQuery.Search));
            }

            // Get the total count of roles after applying filters
            var totalCount = await query.CountAsync(cancellationToken);

            // Handle sorting
            var sortField = request.GridQuery.SortField ?? nameof(IdentityRole<int>.Name);
            query = QuerySort(query, sortField, request.GridQuery.SortDir);

            // Set pagination values (page number and page size)
            var page = request.GridQuery.Page ?? 0;
            var pageSize = request.GridQuery.PageSize ?? 20;

            // Fetch the roles based on pagination
            var roles = await query.Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken);

            // Create a paged list of roles
            var pagedList = new PagedList<IdentityRole<int>>(totalCount, roles);

            // Return the success response with paged list of roles
            return new ResponseWrapper<PagedList<IdentityRole<int>>>().Success(pagedList);
        }

        IQueryable<IdentityRole<int>> QuerySort(IQueryable<IdentityRole<int>> query, string sortField, DataGridQuerySortDirection sortDirection)
        {
            switch (sortField)
            {
                case nameof(IdentityRole<int>.Name):
                    return sortDirection == DataGridQuerySortDirection.Ascending
                        ? query.OrderBy(r => r.Name)
                        : query.OrderByDescending(r => r.Name);
                default:
                    return sortDirection == DataGridQuerySortDirection.Ascending
                        ? query.OrderBy(r => r.Id)
                        : query.OrderByDescending(r => r.Id);
            }
        }
    }
}
