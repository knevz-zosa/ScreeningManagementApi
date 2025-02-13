using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Examinations;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.AssessmentsResult.Exams.Commands;
public class UpdateExamResultCommand : BaseUpdateCommand<ExaminationResultUpdate>
{
	public UpdateExamResultCommand(ExaminationResultUpdate update)
	{
		Update = update;
	}
}

public class UpdateExamResultCommandHandler : BaseUpdateCommandHandler<UpdateExamResultCommand, ExaminationResultUpdate>
{
	public UpdateExamResultCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public async override Task<ResponseWrapper<int>> Handle(UpdateExamResultCommand command, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<Examination>().GetAsync(command.Update.Id);
        if (resultInDb == null)
            return new ResponseWrapper<int>().Failed("Result does not exists.");

        var result = resultInDb.Update(command.Update.ReadingRawScore, command.Update.MathRawScore,
            command.Update.ScienceRawScore, command.Update.IntelligenceRawScore, command.Update.UpdatedBy);
        await _unitOfWork.WriteRepositoryFor<Examination>().UpdateAsync(result);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(result.Id, "Result updated successfuly.");
    }
}
