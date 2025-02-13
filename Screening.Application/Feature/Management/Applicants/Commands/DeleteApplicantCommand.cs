using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Applicants.Commands;
public class DeleteApplicantCommand : BaseDeleteCommand { }
public class DeleteApplicantCommandHandler : BaseDeleteCommandHandler<DeleteApplicantCommand>
{
    public DeleteApplicantCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(DeleteApplicantCommand command, CancellationToken cancellationToken)
    {
        var model = await _unitOfWork.ReadRepositoryFor<Applicant>().GetAsync(command.Id);

        if (model == null)
        {
            return new ResponseWrapper<int>().Failed("Applicant does not exist.");
        }

        await _unitOfWork.WriteRepositoryFor<Applicant>().DeleteAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id, "Applicant deleted successfully.");
    }
}
