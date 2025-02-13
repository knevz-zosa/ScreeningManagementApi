using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Campuses;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Campuses.Commands;
public class CreateCampusCommand : BaseCreateCommand<CampusRequest>
{
    public CreateCampusCommand(CampusRequest request)
    {
        Request = request;
    }
}
public class CreateCampusCommandHandler : BaseCreateCommandHandler<CreateCampusCommand, CampusRequest>
{
    public CreateCampusCommandHandler(IUnitOfWork<int> unitOfWork)
        : base(unitOfWork) { }

    public override async Task<ResponseWrapper<int>> Handle(CreateCampusCommand command, CancellationToken cancellationToken)
    {
        var trimmedName = command.Request.Name.Trim().ToLower();

        var existingResult = await _unitOfWork.ReadRepositoryFor<Campus>()
            .Entities.FirstOrDefaultAsync(x => x.Name.Trim().ToLower() == trimmedName);

        if (existingResult != null)
            return new ResponseWrapper<int>().Failed(message: "Campus with this name already exists.");

        var model = command.Request.Adapt<Campus>();

        model.Name = model.Name.Trim();
        model.DateCreated = DateTime.Now;

        await _unitOfWork.WriteRepositoryFor<Campus>().CreateAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id, "Campus created successfully.");
    }
}