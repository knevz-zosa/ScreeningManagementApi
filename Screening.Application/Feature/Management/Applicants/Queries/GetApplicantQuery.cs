using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Applicants;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Applicants.Queries;
public class GetApplicantQuery : BaseGetQuery<ApplicantResponse>
{
    public GetApplicantQuery(int id) : base(id) { }
}
public class GetApplicantQueryHandler : BaseGetQueryHandler<GetApplicantQuery, ApplicantResponse>
{
    public GetApplicantQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public override async Task<ResponseWrapper<ApplicantResponse>> Handle(GetApplicantQuery get, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<Applicant>().Entities            
           .AsNoTracking()
           .ProjectTo<ApplicantResponse>(_mapper.ConfigurationProvider)
           .SingleOrDefaultAsync(x => x.Id == get.Id, cancellationToken);

        if (resultInDb == null)
            return new ResponseWrapper<ApplicantResponse>().Failed(message: "Applicant does not exists.");

        return new ResponseWrapper<ApplicantResponse>().Success(resultInDb);
    }
}

