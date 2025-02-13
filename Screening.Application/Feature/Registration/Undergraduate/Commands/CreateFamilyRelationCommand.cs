using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Families;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Registration.Undergraduate.Commands;
public class CreateFamilyRelationCommand : BaseCreateCommand<FamilyRelationRequest>
{
    public CreateFamilyRelationCommand(FamilyRelationRequest request)
    {
        Request = request;
    }
}
public class CreateFamilyRelationCommandHandler : BaseCreateCommandHandler<CreateFamilyRelationCommand, FamilyRelationRequest>
{
    public CreateFamilyRelationCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(CreateFamilyRelationCommand command, CancellationToken cancellationToken)
    {
        var result = command.Request;

        var model = result.Adapt<FamilyRelation>();
        await _unitOfWork.WriteRepositoryFor<FamilyRelation>().CreateAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id);
    }
}
