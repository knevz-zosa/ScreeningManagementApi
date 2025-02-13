using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Examinations;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.AssessmentsResult.Exams.Commands;
public class CreateExamResultCommand : BaseCreateCommand<ExaminationResultRequest>
{
	public CreateExamResultCommand(ExaminationResultRequest request)
	{
		Request = request;
	}
}

public class CreateExamResultCommandHandler : BaseCreateCommandHandler<CreateExamResultCommand, ExaminationResultRequest>
{
	public CreateExamResultCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public async override Task<ResponseWrapper<int>> Handle(CreateExamResultCommand command, CancellationToken cancellationToken)
    {
        var result = command.Request;

        var model = result.Adapt<Examination>();
        await _unitOfWork.WriteRepositoryFor<Examination>().CreateAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id, "Exam result has been recorded successfully.");
    }
}
