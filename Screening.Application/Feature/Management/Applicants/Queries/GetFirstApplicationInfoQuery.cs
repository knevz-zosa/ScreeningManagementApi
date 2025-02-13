using AutoMapper;
using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.FirstApplications;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Applicants.Queries;
public class GetFirstApplicationInfoQuery : BaseGetQuery<FirstApplicationInfoResponse>
{
    public GetFirstApplicationInfoQuery(int id) : base(id) { }
}
public class GetFirstApplicationInfoQueryHandler : BaseGetQueryHandler<GetFirstApplicationInfoQuery, FirstApplicationInfoResponse>
{
    public GetFirstApplicationInfoQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public async override Task<ResponseWrapper<FirstApplicationInfoResponse>> Handle(GetFirstApplicationInfoQuery get, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<FirstApplicationInfo>().GetAsync(get.Id);
        if (resultInDb == null)
            return new ResponseWrapper<FirstApplicationInfoResponse>().Failed(message: "Result does not exists.");

        return new ResponseWrapper<FirstApplicationInfoResponse>().Success(data: resultInDb.Adapt<FirstApplicationInfoResponse>());
    }
}
