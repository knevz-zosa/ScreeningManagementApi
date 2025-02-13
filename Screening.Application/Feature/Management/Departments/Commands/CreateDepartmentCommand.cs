using Mapster;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Departments;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Departments.Commands;
public class CreateDepartmentCommand : BaseCreateCommand<DepartmentRequest>
{
    public CreateDepartmentCommand(DepartmentRequest request)
    {
        Request = request;
    }
}
public class CreateDepartmentCommandHandeler : BaseCreateCommandHandler<CreateDepartmentCommand, DepartmentRequest>
{
    public CreateDepartmentCommandHandeler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(CreateDepartmentCommand command, CancellationToken cancellationToken)
    {
        var trimmedName = command.Request.Name.Trim().ToLower();

        var existingResult = await _unitOfWork.ReadRepositoryFor<Department>()
            .Entities.FirstOrDefaultAsync(x => x.Name.Trim().ToLower() == trimmedName
            && x.CampusId == command.Request.CampusId);

        if (existingResult != null)
            return new ResponseWrapper<int>().Failed(message: "Department with this name already exists.");

        var model = command.Request.Adapt<Department>();

        model.Name = model.Name.Trim();

        model.DateCreated = DateTime.Now;
        await _unitOfWork.WriteRepositoryFor<Department>().CreateAsync(model);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(model.Id, "Department created successfully.");
    }
}

