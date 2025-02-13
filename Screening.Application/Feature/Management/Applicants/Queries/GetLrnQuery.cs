using AutoMapper;
using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Academics;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Applicants.Queries;
public class GetLrnQuery : BaseGetQuery<LrnResponse>
{
    public GetLrnQuery(int id) : base(id) { }
}
public class GetLrnQueryHandler : BaseGetQueryHandler<GetLrnQuery, LrnResponse>
{
    public GetLrnQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public override async Task<ResponseWrapper<LrnResponse>> Handle(GetLrnQuery get, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<AcademicBackground>().GetAsync(get.Id);
        if (resultInDb == null)
            return new ResponseWrapper<LrnResponse>().Failed(message: "Result does not exists.");

        return new ResponseWrapper<LrnResponse>().Success(data: resultInDb.Adapt<LrnResponse>());
    }
}
