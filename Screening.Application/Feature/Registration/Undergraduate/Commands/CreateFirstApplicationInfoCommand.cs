using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.FirstApplications;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Registration.Undergraduate.Commands;
public class CreateFirstApplicationInfoCommand : BaseCreateCommand<FirstApplicationInfoRequest>
{
    public CreateFirstApplicationInfoCommand(FirstApplicationInfoRequest request)
    {
        Request = request;
    }
}
public class CreateFirstApplicationInfoCommandHandler : BaseCreateCommandHandler<CreateFirstApplicationInfoCommand, FirstApplicationInfoRequest>
{
    public CreateFirstApplicationInfoCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(CreateFirstApplicationInfoCommand command, CancellationToken cancellationToken)
    {
        var result = command.Request;

        var model = result.Adapt<FirstApplicationInfo>();
        await _unitOfWork.WriteRepositoryFor<FirstApplicationInfo>().CreateAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id);
    }
}