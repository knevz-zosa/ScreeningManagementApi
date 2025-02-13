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
public class ListPassersQuery : BaseListQuery<ApplicantResponse>
{
    public string Access { get; set; }
}
public class ListPassersQueryHandler : BaseListQueryHandler<ListPassersQuery, ApplicantResponse>
{
    public ListPassersQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public override async Task<ResponseWrapper<PagedList<ApplicantResponse>>> Handle(ListPassersQuery list, CancellationToken cancellationToken)
    {
        var query = _unitOfWork.ReadRepositoryFor<Applicant>().Entities
           .AsNoTracking()
           .Where(x => x.Registered != null)
           .ProjectTo<ApplicantResponse>(_mapper.ConfigurationProvider);

        if (list.Access != "All")
        {
            query = query.Where(x => x.Schedule.Campus.Name == list.Access);
        }
      
        query = query.Where(x => Utility.Remarks(x) == "Passed");

        if (!string.IsNullOrEmpty(list.GridQuery.Search))
        {
            var searchTerm = list.GridQuery.Search.ToLower();

            query = query.Where(c =>
                c.PersonalInformation.LastName.ToLower().Contains(searchTerm) ||
                c.PersonalInformation.LastName.ToLower() == searchTerm ||
                c.PersonalInformation.FirstName.ToLower().Contains(searchTerm) ||
                c.PersonalInformation.FirstName.ToLower() == searchTerm ||
                c.Schedule.Campus.Name.ToLower().Contains(searchTerm) ||
                c.Schedule.Campus.Name.ToLower() == searchTerm ||
                c.Schedule.Campus.Courses.Any(course =>
                    course.Id == c.CourseId &&
                    (course.Name.ToLower().Contains(searchTerm) || course.Name.ToLower() == searchTerm)));
        }

        // SchoolYear filtering in the query
        if (!string.IsNullOrEmpty(list.GridQuery.SchoolYear))
        {
            query = query.Where(c => c.Schedule.SchoolYear.Equals(list.GridQuery.SchoolYear));
        }

        var sortField = list.GridQuery.SortField ?? nameof(ApplicantResponse.PersonalInformation.LastName);
        query = QuerySort(query.AsQueryable(), sortField, list.GridQuery.SortDir);

        var totalCount = query.Count();

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
            case "LastName":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.PersonalInformation.LastName)
                    : query.OrderByDescending(c => c.PersonalInformation.LastName);
            case "PersonalInformation.LastName":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.PersonalInformation.LastName)
                    : query.OrderByDescending(c => c.PersonalInformation.LastName);
            case "OverallTotalRating":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => double.Parse(Utility.OverallTotalRating(c)))
                    : query.OrderByDescending(c => double.Parse(Utility.OverallTotalRating(c)));
            case "ExaminationResult":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => double.Parse(Utility.ExaminationResult(c)))
                    : query.OrderByDescending(c => double.Parse(Utility.ExaminationResult(c)));
            case "InterviewResult":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => double.Parse(Utility.InterviewResult(c)))
                    : query.OrderByDescending(c => double.Parse(Utility.InterviewResult(c)));
            case "CourseAliasName":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => Utility.CoursealiasName(c))
                    : query.OrderByDescending(c => Utility.CoursealiasName(c));
            case "Schedule.Campus.Name":
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c.Schedule.Campus.Name)
                    : query.OrderByDescending(c => c.Schedule.Campus.Name);
            default:
                return sortDirection == DataGridQuerySortDirection.Ascending
                    ? query.OrderBy(c => c)
                    : query.OrderByDescending(c => c.Id);
        }
    }
}
