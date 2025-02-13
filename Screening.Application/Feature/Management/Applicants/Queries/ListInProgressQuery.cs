using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Extensions;
using Screening.Common.Models.Applicants;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Applicants.Queries;
public class ListInProgressQuery : BaseListQuery<ApplicantResponse>
{
    public string Access { get; set; }
}
public class ListInProgressQueryHandler : BaseListQueryHandler<ListInProgressQuery, ApplicantResponse>
{
    public ListInProgressQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public override async Task<ResponseWrapper<PagedList<ApplicantResponse>>> Handle(ListInProgressQuery list, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.ReadRepositoryFor<Applicant>().Entities;
        var data = repository;

        if (list.Access != "All")
        {
            data = data
            .AsNoTracking()
            .Where(x => x.Schedule.Campus.Name == list.Access);
        }
        else
        {
            data = data
            .AsNoTracking();
        }
        data = data.Where(a => a.Registered == null);
        var query = data.ProjectTo<ApplicantResponse>(_mapper.ConfigurationProvider);

        if (!string.IsNullOrEmpty(list.GridQuery.Search))
        {
            query = query.Where(c =>
                 c.PersonalInformation.LastName.Contains(list.GridQuery.Search) ||
                 c.PersonalInformation.FirstName.Contains(list.GridQuery.Search) ||
                 c.Schedule.Campus.Name.Contains(list.GridQuery.Search) ||
                 c.Schedule.Campus.Courses
                    .Any(course => course.Id == c.CourseId && course.Name.Contains(list.GridQuery.Search)));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var sortField = list.GridQuery.SortField ?? nameof(Applicant.TransactionDate);
        query = QuerySort(query, sortField, list.GridQuery.SortDir);

        var page = list.GridQuery.Page ?? 0;
        var size = list.GridQuery.PageSize ?? 20;
        var models = await query.Skip(page * size).Take(size).ToListAsync(cancellationToken);

        var pagedList = new PagedList<ApplicantResponse>(totalCount, models);

        return new ResponseWrapper<PagedList<ApplicantResponse>>().Success(pagedList);
    }
    IQueryable<ApplicantResponse> QuerySort(IQueryable<ApplicantResponse> query, string sortField, DataGridQuerySortDirection sortDirection)
    {
        switch (sortField)
        {
            case "Name":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.PersonalInformation.LastName)
                    : query.OrderByDescending(c => c.PersonalInformation.LastName);
            case "Schedule.Campus.Name":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Schedule.Campus.Name)
                    : query.OrderByDescending(c => c.Schedule.Campus.Name);
            case "Schedule.Campus.Courses":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Schedule.Campus.Courses.Select(x => x.Name).SingleOrDefault())
                    : query.OrderByDescending(c => c.Schedule.Campus.Courses.Select(x => x.Name).SingleOrDefault());
            case nameof(ApplicantResponse.TransactionDate):
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.TransactionDate)
                    : query.OrderByDescending(c => c.TransactionDate);
            default:
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Id)
                    : query.OrderByDescending(c => c.Id);
        }
    }
}
