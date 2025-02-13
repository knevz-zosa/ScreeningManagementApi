using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.CouncelorConsultations;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Registration.Undergraduate.Commands;
public class CreateCounselorConsultationCommand : BaseCreateCommand<CounselorConsultationRequest>
{
    public CreateCounselorConsultationCommand(CounselorConsultationRequest request)
    {
        Request = request;
    }
}
public class CreateCounselorConsultationCommandHandler : BaseCreateCommandHandler<CreateCounselorConsultationCommand, CounselorConsultationRequest>
{
    public CreateCounselorConsultationCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(CreateCounselorConsultationCommand command, CancellationToken cancellationToken)
    {
        var result = command.Request;

        var model = result.Adapt<CouncelorConsultation>();
        await _unitOfWork.WriteRepositoryFor<CouncelorConsultation>().CreateAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id);
    }
}
