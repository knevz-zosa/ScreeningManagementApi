using AutoMapper;
using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Examinations;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.AssessmentsResult.Exams.Queries;
public class GetExamResultQuery : BaseGetQuery<ExaminationResponse>
{
	public GetExamResultQuery(int id) : base(id) { }
}

public class GetExamResultQueryHandler : BaseGetQueryHandler<GetExamResultQuery, ExaminationResponse>
{
    public GetExamResultQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public async override Task<ResponseWrapper<ExaminationResponse>> Handle(GetExamResultQuery get, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<Examination>().GetAsync(get.Id);
        if (resultInDb == null)
            return new ResponseWrapper<ExaminationResponse>().Failed(message: "Result does not exists.");

        return new ResponseWrapper<ExaminationResponse>().Success(data: resultInDb.Adapt<ExaminationResponse>());
    }
}
