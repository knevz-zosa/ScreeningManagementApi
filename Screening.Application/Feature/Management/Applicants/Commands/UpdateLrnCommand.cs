using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Academics;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Screening.Application.Feature.Management.Applicants.Commands;
public class UpdateLrnCommand : BaseUpdateCommand<LrnUpdate>
{
    public UpdateLrnCommand(LrnUpdate update)
    {
        Update = update;
    }
}
public class UpdateLrnCommandHandler : BaseUpdateCommandHandler<UpdateLrnCommand, LrnUpdate>
{
    public UpdateLrnCommandHandler(IUnitOfWork<int> unitOfWork) : base(unitOfWork) { }

    public override async Task<ResponseWrapper<int>> Handle(UpdateLrnCommand command, CancellationToken cancellationToken)
    {        
        var result = command.Update;

        var resultExist = await _unitOfWork.ReadRepositoryFor<Registered>().Entities
            .Include(x => x.Applicant)
                .ThenInclude(x => x.AcademicBackground)
            .AsNoTracking()
           .AnyAsync(x => x.ApplicantId != x.Applicant.Id && x.Applicant.AcademicBackground.LRN == result.LRN, cancellationToken);

        if (resultExist)
            return new ResponseWrapper<int>().Failed(message: "Applicant with the same LRN already exists.");

        var resultInDb = await _unitOfWork.ReadRepositoryFor<AcademicBackground>().GetAsync(command.Update.Id);

        if (resultInDb == null)
            return new ResponseWrapper<int>().Failed("Applicant does not exists.");

        resultInDb.Update(result.LRN);

        await _unitOfWork.WriteRepositoryFor<AcademicBackground>().UpdateAsync(resultInDb);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new ResponseWrapper<int>().Success(result.Id, "LRN updated successfuly.");
    }
}
