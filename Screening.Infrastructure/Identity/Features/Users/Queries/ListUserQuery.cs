using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Screening.Common.Extensions;
using Screening.Common.Models.Users;
using Screening.Common.Wrapper;
using Screening.Infrastructure.Context;
using Screening.Infrastructure.Models;

namespace Screening.Infrastructure.Identity.Features.Users.Queries;
public class ListUserQuery :  IRequest<ResponseWrapper<PagedList<UserResponse>>>
{
    public DataGridQuery GridQuery { get; set; }
}

public class ListUserQueryHandler : IRequestHandler<ListUserQuery, ResponseWrapper<PagedList<UserResponse>>>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<int>> _roleManager;
    private readonly ApplicationDbContext _context;
    public ListUserQueryHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, ApplicationDbContext context)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _context = context;
    }
    public async Task<ResponseWrapper<PagedList<UserResponse>>> Handle(ListUserQuery request, CancellationToken cancellationToken)
    {
        var query = _userManager.Users.AsQueryable();

        if (!string.IsNullOrEmpty(request.GridQuery.Search))
        {
            query = query.Where(u => u.FirstName.Contains(request.GridQuery.Search) || u.LastName.Contains(request.GridQuery.Search));
        }

        // Get the total count of users after applying filters
        var totalCount = await query.CountAsync(cancellationToken);

        // Handle sorting
        var sortField = request.GridQuery.SortField ?? nameof(ApplicationUser.LastName);
        query = QuerySort(query, sortField, request.GridQuery.SortDir);

        // Set pagination values (page number and page size)
        var page = request.GridQuery.Page ?? 0;
        var pageSize = request.GridQuery.PageSize ?? 20;

        // Fetch the users based on pagination
        var users = await query.Skip(page * pageSize).Take(pageSize).ToListAsync(cancellationToken);

        // Map the users to UserResponse objects
        var userResponses = new List<UserResponse>();

        foreach (var user in users)
        {
            // Get the roles associated with each user
            var roles = await _userManager.GetRolesAsync(user);

            // Map user data to UserResponse and include roles and access
            var userResponse = new UserResponse
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Role = roles.FirstOrDefault(),
                Access = user.Access,
                IsActive = user.IsActive
            };

            userResponses.Add(userResponse);
        }

        // Create a paged list of user responses
        var pagedList = new PagedList<UserResponse>(totalCount, userResponses);

        // Return the success response with paged list
        return new ResponseWrapper<PagedList<UserResponse>>().Success(pagedList);
    }

    IQueryable<ApplicationUser> QuerySort(IQueryable<ApplicationUser> query, string sortField, DataGridQuerySortDirection sortDirection)
        {
        switch (sortField)
        {
            case nameof(ApplicationUser.LastName):
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.LastName)
                    : query.OrderByDescending(c => c.LastName);
            case nameof(ApplicationUser.Access):
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Access)
                    : query.OrderByDescending(c => c.Access);
            case "Role":
                var roleQuery = _context.Users
                    .Join(_context.UserRoles,
                        user => user.Id,
                        userRole => userRole.UserId,
                        (user, userRole) => new { user, userRole })
                    .Join(_context.Roles,
                        combined => combined.userRole.RoleId,
                        role => role.Id,
                        (combined, role) => new { combined.user, role });
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? roleQuery.OrderBy(r => r.role.Name).Select(r => r.user)
                    : roleQuery.OrderByDescending(r => r.role.Name).Select(r => r.user);
            default:
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Id)
                    : query.OrderByDescending(c => c.Id);
        }
    }  
}
