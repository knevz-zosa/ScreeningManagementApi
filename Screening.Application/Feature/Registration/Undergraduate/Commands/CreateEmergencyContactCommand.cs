using Mapster;
using MediatR;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.EmergencyContacts;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Registration.Undergraduate.Commands;
public class CreateEmergencyContactCommand : BaseCreateCommand<EmergencyContactRequest>
{
    public CreateEmergencyContactCommand(EmergencyContactRequest request)
    {
        Request = request;
    }
}
public class CreateEmergencyContactCommandHandler : BaseCreateCommandHandler<CreateEmergencyContactCommand, EmergencyContactRequest>
{
    public CreateEmergencyContactCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(CreateEmergencyContactCommand command, CancellationToken cancellationToken)
    {
        var result = command.Request;

        var model = result.Adapt<EmergencyContact>();
        await _unitOfWork.WriteRepositoryFor<EmergencyContact>().CreateAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id);
    }
}
