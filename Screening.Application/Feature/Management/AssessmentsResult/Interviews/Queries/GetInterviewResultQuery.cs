using AutoMapper;
using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Interviews;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.AssessmentsResult.Interviews.Queries;
public class GetInterviewResultQuery : BaseGetQuery<InterviewResponse>
{
    public GetInterviewResultQuery(int id) : base(id) { }
}
public class GetInterviewResultQueryHandler : BaseGetQueryHandler<GetInterviewResultQuery, InterviewResponse>
{
    public GetInterviewResultQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public override async Task<ResponseWrapper<InterviewResponse>> Handle(GetInterviewResultQuery get, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<Interview>().GetAsync(get.Id);
        if (resultInDb == null)
            return new ResponseWrapper<InterviewResponse>().Failed(message: "Result does not exists.");

        return new ResponseWrapper<InterviewResponse>().Success(data: resultInDb.Adapt<InterviewResponse>());
    }
}
