using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Extensions;
using Screening.Common.Models.Campuses;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Campuses.Queries;
public class ListCampusQuery : BaseListQuery<CampusResponse> { }

public class ListCampusQueryHandler : BaseListQueryHandler<ListCampusQuery, CampusResponse>
{
    public ListCampusQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
        : base(unitOfWork, mapper) { }

    public override async Task<ResponseWrapper<PagedList<CampusResponse>>> Handle(ListCampusQuery list, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.ReadRepositoryFor<Campus>().Entities
            .AsNoTracking()
            .ProjectTo<CampusResponse>(_mapper.ConfigurationProvider);

        var query = repository;

        if (!string.IsNullOrEmpty(list.GridQuery.Search))
        {
            query = query.Where(u => u.Name.Contains(list.GridQuery.Search));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var sortField = list.GridQuery.SortField ?? nameof(CampusResponse.Name);
        query = QuerySort(query, sortField, list.GridQuery.SortDir);

        var page = list.GridQuery.Page ?? 0;
        var size = list.GridQuery.PageSize ?? 20;
        var models = await query.Skip(page * size).Take(size).ToListAsync(cancellationToken);

        var pagedList = new PagedList<CampusResponse>(totalCount, models);
        return new ResponseWrapper<PagedList<CampusResponse>>().Success(pagedList);
    }

    IQueryable<CampusResponse> QuerySort(IQueryable<CampusResponse> query, string sortField, DataGridQuerySortDirection sortDirection)
    {
        switch (sortField)
        {
            case nameof(Campus.Name):
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Name)
                    : query.OrderByDescending(c => c.Name);
            case nameof(CampusResponse.DateCreated):
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
