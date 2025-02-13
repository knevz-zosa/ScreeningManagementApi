using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Interviews;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.AssessmentsResult.Interviews.Commands;
public class UpdateInterviewActiveCommand : BaseUpdateCommand<InterviewActiveUpdate>
{
    public UpdateInterviewActiveCommand(InterviewActiveUpdate update)
    {
        Update = update;
    }
}

public class UpdateInterviewActiveCommandHandler : BaseUpdateCommandHandler<UpdateInterviewActiveCommand, InterviewActiveUpdate>
{
    public UpdateInterviewActiveCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(UpdateInterviewActiveCommand command, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<Interview>().GetAsync(command.Update.Id);

        if (resultInDb == null)
            return new ResponseWrapper<int>().Failed("Interview does not exists.");

        var result = resultInDb.UpdateIsActive(command.Update.IsUse, command.Update.UpdatedBy);

        await _unitOfWork.WriteRepositoryFor<Interview>().UpdateAsync(result);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(result.Id, "Interview has been selected as active");
    }
}
