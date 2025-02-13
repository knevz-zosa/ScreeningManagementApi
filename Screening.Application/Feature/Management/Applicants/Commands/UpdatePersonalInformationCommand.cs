using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.PersonalInformations;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Screening.Application.Feature.Management.Applicants.Commands;
public class UpdatePersonalInformationCommand : BaseUpdateCommand<PersonalInformationUpdate>
{
    public UpdatePersonalInformationCommand(PersonalInformationUpdate update)
    {
        Update = update;
    }
}
public class UpdatePersonalInformationCommandHandler : BaseUpdateCommandHandler<UpdatePersonalInformationCommand, PersonalInformationUpdate>
{
    public UpdatePersonalInformationCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }
    public override async Task<ResponseWrapper<int>> Handle(UpdatePersonalInformationCommand command, CancellationToken cancellationToken)
    {
        var trimmedFirstName = command.Update.FirstName.Trim().ToLower();
        var trimmedMiddleName = command.Update.MiddleName?.Trim().ToLower();
        var trimmedLastName = command.Update.LastName.Trim().ToLower();

        var existingResult = await _unitOfWork.ReadRepositoryFor<Applicant>().Entities
            .AsNoTracking()
            .AnyAsync(a => a.Registered != null &&
                a.PersonalInformation.FirstName.Trim().ToLower() == trimmedFirstName &&
                (a.PersonalInformation.MiddleName == null
                    ? trimmedMiddleName == null
                    : a.PersonalInformation.MiddleName.Trim().ToLower() == trimmedMiddleName) &&
                a.PersonalInformation.LastName.Trim().ToLower() == trimmedLastName &&
                a.PersonalInformation.DateofBirth.Date == command.Update.DateofBirth.Date &&
                a.Id != command.Update.ApplicantId, cancellationToken);

        if (existingResult)
            return new ResponseWrapper<int>().Failed(message: "Applicant with this name already exists.");

        var resultInDb = await _unitOfWork.ReadRepositoryFor<PersonalInformation>().GetAsync(command.Update.Id);

        if (resultInDb == null)
            return new ResponseWrapper<int>().Failed("Personal information does not exist.");

        resultInDb.Update(
                command.Update.ApplicantId,
                command.Update.FirstName.Trim(),
                command.Update.MiddleName?.Trim() ?? string.Empty,
                command.Update.LastName.Trim(),
                command.Update.NameExtension?.Trim() ?? string.Empty,
                command.Update.NickName?.Trim() ?? string.Empty,
                command.Update.Sex,
                command.Update.DateofBirth,
                command.Update.PlaceOfBirth?.Trim() ?? string.Empty,
                command.Update.Citizenship?.Trim() ?? string.Empty,
                command.Update.Email?.Trim() ?? string.Empty,
                command.Update.ContactNumber?.Trim() ?? string.Empty,
                command.Update.HouseNo?.Trim() ?? string.Empty,
                command.Update.Street?.Trim() ?? string.Empty,
                command.Update.Barangay?.Trim() ?? string.Empty,
                command.Update.Purok?.Trim() ?? string.Empty,
                command.Update.Municipality?.Trim() ?? string.Empty,
                command.Update.Province?.Trim() ?? string.Empty,
                command.Update.ZipCode?.Trim() ?? string.Empty
            );

        await _unitOfWork.WriteRepositoryFor<PersonalInformation>().UpdateAsync(resultInDb);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(resultInDb.Id, "Personal information updated successfully.");
    }
}
