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
public class ListApplicantQuery : BaseListQuery<ApplicantResponse>
{
    public int? ScheduleId { get; set; }
    public string Access { get; set; }
}
public class ListApplicantQueryHandler : BaseListQueryHandler<ListApplicantQuery, ApplicantResponse>
{
    public ListApplicantQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public override async Task<ResponseWrapper<PagedList<ApplicantResponse>>> Handle(ListApplicantQuery list, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.ReadRepositoryFor<Applicant>().Entities
       .AsNoTracking()
       .Where(x => x.Registered != null)
       .ProjectTo<ApplicantResponse>(_mapper.ConfigurationProvider);       

        // Handle the Access and ScheduleId filtering logic
        if (list.Access != "All")
        {
            query = query.Where(x => x.Schedule.Campus.Name == list.Access);

            if (list.ScheduleId.HasValue && list.ScheduleId.Value != 0)
            {
                query = query.Where(x => x.ScheduleId == list.ScheduleId.Value);
            }
        }
        else if (list.ScheduleId.HasValue && list.ScheduleId.Value != 0)
        {
            query = query.Where(x => x.ScheduleId == list.ScheduleId.Value);
        }

        // Search functionality in the query
        if (!string.IsNullOrEmpty(list.GridQuery.Search))
        {
            var searchTerm = list.GridQuery.Search.ToLower();
            query = query.Where(c =>
                c.PersonalInformation.LastName.Contains(searchTerm) ||
                c.PersonalInformation.FirstName.Contains(searchTerm) ||
                c.Schedule.Campus.Name.Contains(searchTerm) ||
                c.Schedule.Campus.Courses
                    .Any(course => course.Id == c.CourseId && course.Name.Contains(searchTerm))
            );
        }

        // SchoolYear filtering in the query
        if (!string.IsNullOrEmpty(list.GridQuery.SchoolYear))
        {
            query = query.Where(c => c.Schedule.SchoolYear.Equals(list.GridQuery.SchoolYear));
        }

        // Sorting in the query
        var sortField = list.GridQuery.SortField ?? nameof(ApplicantResponse.PersonalInformation.LastName);
        query = QuerySort(query, sortField, list.GridQuery.SortDir);

        // Paging
        var totalCount = await query.CountAsync(cancellationToken);
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
            case "PersonalInformation.LastName":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.PersonalInformation.LastName)
                    : query.OrderByDescending(c => c.PersonalInformation.LastName);
            case nameof(ApplicantResponse.Schedule):
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Schedule.Campus.Name)
                    : query.OrderByDescending(c => c.Schedule.Campus.Name);
            default:
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Id)
                    : query.OrderByDescending(c => c.Id);
        }
    }
}

