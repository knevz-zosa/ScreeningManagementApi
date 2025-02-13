using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Extensions;
using Screening.Common.Models.Interviews;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.AssessmentsResult.Interviews.Queries;
public class ListInterviewResultQuery : BaseListQuery<InterviewResponse> { }
public class ListInterviewResultQueryHandler : BaseListQueryHandler<ListInterviewResultQuery, InterviewResponse>
{
    public ListInterviewResultQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public override async Task<ResponseWrapper<PagedList<InterviewResponse>>> Handle(ListInterviewResultQuery list, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.ReadRepositoryFor<Interview>();

        var query = repository.Entities.ProjectTo<InterviewResponse>(_mapper.ConfigurationProvider);

        if (!string.IsNullOrEmpty(list.GridQuery.Search))
        {
            query = query.Where(c => c.InterviewDate.ToShortDateString().Contains(list.GridQuery.Search));
        }
        var totalCount = await query.CountAsync();

        var sortField = list.GridQuery.SortField ?? nameof(InterviewResponse.InterviewDate);
        query = QuerySort(query, sortField, list.GridQuery.SortDir);

        var page = list.GridQuery.Page ?? 0;
        var size = list.GridQuery.PageSize ?? 20;
        var models = await query.Skip(page * size).Take(size).ToListAsync();

        var pagedList = new PagedList<InterviewResponse>(totalCount, models);

        return new ResponseWrapper<PagedList<InterviewResponse>>().Success(pagedList);
    }
    IQueryable<InterviewResponse> QuerySort(IQueryable<InterviewResponse> query, string sortField, DataGridQuerySortDirection sortDirection)
    {
        switch (sortField)
        {
            case nameof(InterviewResponse.InterviewDate):
                return sortDirection == DataGridQuerySortDirection.Ascending
                      ? query.OrderBy(c => c.InterviewDate)
                      : query.OrderByDescending(c => c.InterviewDate);
            case nameof(InterviewResponse.DateRecorded):
                return sortDirection == DataGridQuerySortDirection.Ascending
                     ? query.OrderBy(c => c.DateRecorded)
                     : query.OrderByDescending(c => c.DateRecorded);
            default:
                return sortDirection == DataGridQuerySortDirection.Ascending
                     ? query.OrderBy(c => c.Id)
                     : query.OrderByDescending(c => c.Id);
        }
    }
}

