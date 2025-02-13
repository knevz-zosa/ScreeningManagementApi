using MediatR;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Wrapper;

namespace Screening.Application.Feature.Management.Schedules.Queries;

public class ListSchoolYearsQuery : IRequest<ResponseWrapper<List<string>>> { }
public class GetSchoolYearsQueryHandler : IRequestHandler<ListSchoolYearsQuery, ResponseWrapper<List<string>>>
{
    private readonly IUnitOfWork<int> _unitOfWork;

    public GetSchoolYearsQueryHandler(IUnitOfWork<int> unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseWrapper<List<string>>> Handle(ListSchoolYearsQuery request, CancellationToken cancellationToken)
    {
        var schoolYears = await _unitOfWork.ReadRepositoryFor<Schedule>()
            .Entities
            .Select(x => x.SchoolYear)
            .Distinct()
            .ToListAsync(cancellationToken);

        return new ResponseWrapper<List<string>>().Success(schoolYears);
    }
}