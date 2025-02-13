using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.PhysicalHealths;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Registration.Undergraduate.Commands;
public class CreatePhysicalHealthCommand : BaseCreateCommand<PhysicalHealthRequest>
{
    public CreatePhysicalHealthCommand(PhysicalHealthRequest request)
    {
        Request = request;
    }
}
public class CreatePhysicalHealthCommandHandler : BaseCreateCommandHandler<CreatePhysicalHealthCommand, PhysicalHealthRequest>
{
    public CreatePhysicalHealthCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(CreatePhysicalHealthCommand command, CancellationToken cancellationToken)
    {
        var result = command.Request;

        var model = result.Adapt<PhysicalHealth>();
        await _unitOfWork.WriteRepositoryFor<PhysicalHealth>().CreateAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id);
    }
}
