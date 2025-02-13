using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Extensions;
using Screening.Common.Models.Departments;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Departments.Queries;
public class ListDepartmentQuery : BaseListQuery<DepartmentResponse> { }

public class ListDepartmentQueryHandler : BaseListQueryHandler<ListDepartmentQuery, DepartmentResponse>
{
    public ListDepartmentQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper) { }
    public override async Task<ResponseWrapper<PagedList<DepartmentResponse>>> Handle(ListDepartmentQuery list, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.ReadRepositoryFor<Department>().Entities
            .Include(x => x.Campus)
            .AsNoTracking().
            ProjectTo<DepartmentResponse>(_mapper.ConfigurationProvider);

        var query = repository;

        if (!string.IsNullOrEmpty(list.GridQuery.Search))
        {
            query = query.Where(c => c.Name.Contains(list.GridQuery.Search));
        }

        var totalCount = await query.CountAsync();

        var sortField = list.GridQuery.SortField ?? nameof(DepartmentResponse.Name);
        query = QuerySort(query, sortField, list.GridQuery.SortDir);

        var page = list.GridQuery.Page ?? 0;
        var size = list.GridQuery.PageSize ?? 20;
        var models = await query.Skip(page * size).Take(size).ToListAsync();

        var pagedList = new PagedList<DepartmentResponse>(totalCount, models);
        return new ResponseWrapper<PagedList<DepartmentResponse>>().Success(pagedList);
    }
    IQueryable<DepartmentResponse> QuerySort(IQueryable<DepartmentResponse> query, string sortField, DataGridQuerySortDirection sortDirection)
    {
        switch (sortField)
        {
            case nameof(DepartmentResponse.Name):
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Name)
                    : query.OrderByDescending(c => c.Name);
            case "Campus":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Campus.Name)
                    : query.OrderByDescending(c => c.Campus.Name);
            case nameof(DepartmentResponse.DateCreated):
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.DateCreated)
                    : query.OrderByDescending(c => c.DateCreated);
            default:
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Id)
                    : query.OrderByDescending(c => c.Id);
        }
    }
}
