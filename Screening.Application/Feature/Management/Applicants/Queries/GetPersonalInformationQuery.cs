using AutoMapper;
using Mapster;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.PersonalInformations;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Applicants.Queries;
public class GetPersonalInformationQuery : BaseGetQuery<PersonalInformationResponse>
{
    public GetPersonalInformationQuery(int id) : base(id) { }
}
public class GetPersonalInformationQueryHandler : BaseGetQueryHandler<GetPersonalInformationQuery, PersonalInformationResponse>
{
    public GetPersonalInformationQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public override async Task<ResponseWrapper<PersonalInformationResponse>> Handle(GetPersonalInformationQuery get, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<PersonalInformation>().GetAsync(get.Id);
        if (resultInDb == null)
            return new ResponseWrapper<PersonalInformationResponse>().Failed(message: "Result does not exists.");

        return new ResponseWrapper<PersonalInformationResponse>().Success(data: resultInDb.Adapt<PersonalInformationResponse>());
    }
}
