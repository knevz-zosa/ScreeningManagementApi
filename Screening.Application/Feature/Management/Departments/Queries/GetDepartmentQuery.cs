using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Departments;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Departments.Queries;
public class GetDepartmentQuery : BaseGetQuery<DepartmentResponse>
{
    public GetDepartmentQuery(int id) : base(id) { }
}

public class GetDepartmentQueryHandler : BaseGetQueryHandler<GetDepartmentQuery, DepartmentResponse>
{
    public GetDepartmentQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    : base(unitOfWork, mapper) { }

    public override async Task<ResponseWrapper<DepartmentResponse>> Handle(GetDepartmentQuery get, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<Department>().Entities
               .Include(x => x.Campus)
               .AsNoTracking()
               .ProjectTo<DepartmentResponse>(_mapper.ConfigurationProvider)
               .SingleOrDefaultAsync(x => x.Id == get.Id, cancellationToken);

        if (resultInDb == null)
            return new ResponseWrapper<DepartmentResponse>().Failed(message: "Department does not exists.");

        return new ResponseWrapper<DepartmentResponse>().Success(resultInDb);
    }
}
