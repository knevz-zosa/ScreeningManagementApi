using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Applicants;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Screening.Application.Feature.Management.Applicants.Commands;
public class UpdateStudentIdCommand : BaseUpdateCommand<ApplicantUpdateStudentId>
{
    public UpdateStudentIdCommand(ApplicantUpdateStudentId update)
    {
        Update = update;
    }
}
public class UpdateStudentIdCommandHandler : BaseUpdateCommandHandler<UpdateStudentIdCommand, ApplicantUpdateStudentId>
{
    public UpdateStudentIdCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(UpdateStudentIdCommand command, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(command.Update.StudentId))
            return new ResponseWrapper<int>().Failed("Invalid Student Id.");

        if (command.Update.StudentId.Contains(" "))
            return new ResponseWrapper<int>().Failed("Invalid Student Id."); ;

        var resultExist = await _unitOfWork.ReadRepositoryFor<Registered>().Entities
        .FirstOrDefaultAsync(x => x.ApplicantId != command.Update.Id &&
            x.Applicant.StudentId == command.Update.StudentId);

        if (resultExist != null)
            return new ResponseWrapper<int>().Failed("Student Id already exists.");

        var resultInDb = await _unitOfWork.ReadRepositoryFor<Applicant>().GetAsync(command.Update.Id);

        if (resultInDb == null)
            return new ResponseWrapper<int>().Failed("Apllicant does not exists.");

        var studentId = command.Update.StudentId?.Trim();

        var result = resultInDb.UpdateStudentId(studentId);

        await _unitOfWork.WriteRepositoryFor<Applicant>().UpdateAsync(result);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(result.Id, "Student Id updated successfuly.");
    }
}


