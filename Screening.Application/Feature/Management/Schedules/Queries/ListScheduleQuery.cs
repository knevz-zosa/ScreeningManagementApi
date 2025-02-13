using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Extensions;
using Screening.Common.Models.Schedules;
using Screening.Common.Wrapper;

namespace Screening.Application.Feature.Management.Schedules.Queries;
public class ListScheduleQuery : BaseListQuery<ScheduleResponse>
{
    public string Access { get; set; }
}

public class ListScheduleQueryHandler : BaseListQueryHandler<ListScheduleQuery, ScheduleResponse>
{
    public ListScheduleQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public override async Task<ResponseWrapper<PagedList<ScheduleResponse>>> Handle(ListScheduleQuery list, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.ReadRepositoryFor<Schedule>().Entities;
        var data = repository;
        if (list.Access == "All" || list.Access == string.Empty)
        {
            data = repository
             .AsNoTracking();
        }
        else
        {
            data = data
             .Where(x => x.Campus.Name == list.Access)
                .AsNoTracking();
        }

        var query = data.ProjectTo<ScheduleResponse>(_mapper.ConfigurationProvider);

        if (!string.IsNullOrEmpty(list.GridQuery.Search))
        {
            query = query.Where(u => u.Campus.Name.Contains(list.GridQuery.Search));
        }

        if (!string.IsNullOrEmpty(list.GridQuery.SchoolYear))
        {
            query = query.Where(c =>
                 c.SchoolYear == list.GridQuery.SchoolYear);
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var sortField = list.GridQuery.SortField ?? nameof(ScheduleResponse.ScheduleDate);
        query = QuerySort(query, sortField, list.GridQuery.SortDir);

        var page = list.GridQuery.Page ?? 0;
        var size = list.GridQuery.PageSize ?? 20;
        var models = await query.Skip(page * size).Take(size).ToListAsync(cancellationToken);

        var pagedList = new PagedList<ScheduleResponse>(totalCount, models);
        return new ResponseWrapper<PagedList<ScheduleResponse>>().Success(pagedList);
    }
    IQueryable<ScheduleResponse> QuerySort(IQueryable<ScheduleResponse> query, string sortField, DataGridQuerySortDirection sortDirection)
    {
        switch (sortField)
        {
            case nameof(ScheduleResponse.ScheduleDate):
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.ScheduleDate)
                    : query.OrderByDescending(c => c.ScheduleDate);
            case nameof(ScheduleResponse.DateCreated):
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.DateCreated)
                    : query.OrderByDescending(c => c.DateCreated);
            case nameof(ScheduleResponse.Campus):
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Campus.Name)
                    : query.OrderByDescending(c => c.Campus.Name);
            default:
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Id)
                    : query.OrderByDescending(c => c.Id);
        }
    }
}

