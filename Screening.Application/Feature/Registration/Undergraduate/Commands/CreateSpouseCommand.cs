using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Spouses;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Registration.Undergraduate.Commands;
public class CreateSpouseCommand : BaseCreateCommand<SpouseRequest>
{
    public CreateSpouseCommand(SpouseRequest request)
    {
        Request = request;
    }
}
public class CreateSpouseCommandHandler : BaseCreateCommandHandler<CreateSpouseCommand, SpouseRequest>
{
    public CreateSpouseCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(CreateSpouseCommand command, CancellationToken cancellationToken)
    {
        var result = command.Request;

        var model = result.Adapt<Spouse>();
        await _unitOfWork.WriteRepositoryFor<Spouse>().CreateAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id);
    }
}
