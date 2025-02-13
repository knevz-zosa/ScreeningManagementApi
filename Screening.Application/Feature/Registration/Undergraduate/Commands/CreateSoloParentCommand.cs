using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.SoloParents;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Registration.Undergraduate.Commands;
public class CreateSoloParentCommand : BaseCreateCommand<SoloParentRequest>
{
    public CreateSoloParentCommand(SoloParentRequest request)
    {
        Request = request;
    }
}
public class CreateSoloParentCommandHandler : BaseCreateCommandHandler<CreateSoloParentCommand, SoloParentRequest>
{
    public CreateSoloParentCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(CreateSoloParentCommand command, CancellationToken cancellationToken)
    {
        var result = command.Request;

        var model = result.Adapt<SoloParent>();
        await _unitOfWork.WriteRepositoryFor<SoloParent>().CreateAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id);
    }
}
