using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Wrapper;

namespace Screening.Application.Feature.Management.Schedules.Commands;
public class DeleteScheduleCommand : BaseDeleteCommand { }

public class DeleteScheduleCommandHandler : BaseDeleteCommandHandler<DeleteScheduleCommand>
{
    public DeleteScheduleCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }

    public override async Task<ResponseWrapper<int>> Handle(DeleteScheduleCommand command, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<Schedule>().GetAsync(command.Id);

        if (resultInDb == null)
        {
            return new ResponseWrapper<int>().Failed("Schedule does not exists.");
        }

        await _unitOfWork.WriteRepositoryFor<Schedule>().DeleteAsync(resultInDb);
        await _unitOfWork.CommitAsync(cancellationToken);
        return new ResponseWrapper<int>().Success(resultInDb.Id, "Schedule deleted successfully.");
    }
}

