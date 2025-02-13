﻿using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Interviews;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.AssessmentsResult.Interviews.Commands;
public class UpdateInterviewResultCommand : BaseUpdateCommand<InterviewResultUpdate>
{
	public UpdateInterviewResultCommand(InterviewResultUpdate update)
	{
		Update = update;
	}
}

public class UpdateInterviewResultCommandHandler : BaseUpdateCommandHandler<UpdateInterviewResultCommand, InterviewResultUpdate>
{
	public UpdateInterviewResultCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(UpdateInterviewResultCommand command, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<Interview>().GetAsync(command.Update.Id);

        if (resultInDb == null)
            return new ResponseWrapper<int>().Failed("Interview does not exists.");

        var result = resultInDb.UpdateRating(command.Update.InterviewReading, command.Update.InterviewCommunication,
            command.Update.InterviewAnalytical, command.Update.UpdatedBy, command.Update.Interviewer);
        await _unitOfWork.WriteRepositoryFor<Interview>().UpdateAsync(result);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(result.Id, "Interview rating updated successfuly.");
    }
}
