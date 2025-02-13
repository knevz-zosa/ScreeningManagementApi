using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Extensions;
using Screening.Common.Models.Courses;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Courses.Queries;
public class ListCourseQuery : BaseListQuery<CourseResponse>
{
    public string Access { get; set; }
}

public class ListCourseQueryHandler : BaseListQueryHandler<ListCourseQuery, CourseResponse>
{
    public ListCourseQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public override async Task<ResponseWrapper<PagedList<CourseResponse>>> Handle(ListCourseQuery list, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.ReadRepositoryFor<Course>().Entities;
        var data = repository;
        if (list.Access == "All")
        {
            data = repository
               .AsNoTracking();
        }
        else
        {
            data = repository
               .Where(x => x.Campus.Name == list.Access)
               .AsNoTracking();
        }

        var query = data.ProjectTo<CourseResponse>(_mapper.ConfigurationProvider);

        if (!string.IsNullOrEmpty(list.GridQuery.Search))
        {
            query = query.Where(
                x => x.Name.Contains(list.GridQuery.Search) ||
                x.Campus.Name.Contains(list.GridQuery.Search) ||
                (x.Department != null && x.Department.Name.Contains(list.GridQuery.Search)) ||
                (list.GridQuery.Search.Equals("No Department", StringComparison.OrdinalIgnoreCase) && x.Department == null)
                );
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var sortField = list.GridQuery.SortField ?? nameof(CourseResponse.Name);
        query = QuerySort(query, sortField, list.GridQuery.SortDir);

        var page = list.GridQuery.Page ?? 0;
        var size = list.GridQuery.PageSize ?? 20;
        var models = await query.Skip(page * size).Take(size).ToListAsync(cancellationToken);

        var pagedList = new PagedList<CourseResponse>(totalCount, models);
        return new ResponseWrapper<PagedList<CourseResponse>>().Success(pagedList);
    }
    IQueryable<CourseResponse> QuerySort(IQueryable<CourseResponse> query, string sortField, DataGridQuerySortDirection sortDirection)
    {
        switch (sortField)
        {
            case nameof(CourseResponse.Name):
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Name)
                    : query.OrderByDescending(c => c.Name);
            case "Campus":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Campus.Name)
                    : query.OrderByDescending(c => c.Campus.Name);
            case "Department":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Department != null ? c.Department.Name : string.Empty) // Sort nulls as empty
                    : query.OrderByDescending(c => c.Department != null ? c.Department.Name : string.Empty);
            case nameof(CourseResponse.DateCreated):
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
