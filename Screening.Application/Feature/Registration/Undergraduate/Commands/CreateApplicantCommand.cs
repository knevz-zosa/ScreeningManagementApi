using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Extensions;
using Screening.Common.Models.Applicants;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Registration.Undergraduate.Commands;
public class CreateApplicantCommand : BaseCreateCommand<ApplicantRequest>
{
    public CreateApplicantCommand(ApplicantRequest request)
    {
        Request = request;
    }
}
public class CreateApplicantCommandHandler : BaseCreateCommandHandler<CreateApplicantCommand, ApplicantRequest>
{
    public CreateApplicantCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(CreateApplicantCommand command, CancellationToken cancellationToken)
    {
        var result = command.Request;

        var model = result.Adapt<Applicant>();
        model.ControlNo = Utility.GenerateControlNumber();
        await _unitOfWork.WriteRepositoryFor<Applicant>().CreateAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id);
    }
}
