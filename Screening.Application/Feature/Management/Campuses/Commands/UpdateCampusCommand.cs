using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Campuses;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Campuses.Commands;
public class UpdateCampusCommand : BaseUpdateCommand<CampusUpdate>
{
    public UpdateCampusCommand(CampusUpdate update)
    {
        Update = update;
    }
}
public class UpdateCampusCommandHandler : BaseUpdateCommandHandler<UpdateCampusCommand, CampusUpdate>
{
    public UpdateCampusCommandHandler(IUnitOfWork<int> unitOfWork)
    : base(unitOfWork) { }

    public override async Task<ResponseWrapper<int>> Handle(UpdateCampusCommand command, CancellationToken cancellationToken)
    {
        var trimmedName = command.Update.Name.Trim().ToLower();

        var resultExist = await _unitOfWork.ReadRepositoryFor<Campus>()
            .Entities.FirstOrDefaultAsync(x => x.Id != command.Update.Id && x.Name.Trim().ToLower() == trimmedName);

        if (resultExist != null)
        {
            return new ResponseWrapper<int>().Failed("Campus name already exists.");
        }

        var resultInDb = await _unitOfWork.ReadRepositoryFor<Campus>().GetAsync(command.Update.Id);

        if (resultInDb == null)
            return new ResponseWrapper<int>().Failed("Campus does not exist.");

        resultInDb.Update(command.Update.Name.Trim(), command.Update.Address,
               command.Update.HasDepartment, command.Update.UpdatedBy);

        await _unitOfWork.WriteRepositoryFor<Campus>().UpdateAsync(resultInDb);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(resultInDb.Id, "Campus updated successfully.");
    }
}
