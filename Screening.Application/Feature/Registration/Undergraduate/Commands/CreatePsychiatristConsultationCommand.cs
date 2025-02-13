using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.PsychiatristConsultations;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Registration.Undergraduate.Commands;
public class CreatePsychiatristConsultationCommand : BaseCreateCommand<PsychiatristConsultationRequest>
{
    public CreatePsychiatristConsultationCommand(PsychiatristConsultationRequest request)
    {
        Request = request;
    }
}
public class CreatePsychiatristConsultationCommandHandler : BaseCreateCommandHandler<CreatePsychiatristConsultationCommand, PsychiatristConsultationRequest>
{
    public CreatePsychiatristConsultationCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(CreatePsychiatristConsultationCommand command, CancellationToken cancellationToken)
    {
        var result = command.Request;

        var model = result.Adapt<PsychiatristConsultation>();
        await _unitOfWork.WriteRepositoryFor<PsychiatristConsultation>().CreateAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id);
    }
}
