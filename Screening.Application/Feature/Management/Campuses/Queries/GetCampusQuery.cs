using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Campuses;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Campuses.Queries;
public class GetCampusQuery : BaseGetQuery<CampusResponse>
{
    public GetCampusQuery(int id) : base(id) { }
}

public class GetCampusQueryHandler : BaseGetQueryHandler<GetCampusQuery, CampusResponse>
{
    public GetCampusQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper)
    : base(unitOfWork, mapper) { }

    public override async Task<ResponseWrapper<CampusResponse>> Handle(GetCampusQuery get, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<Campus>().Entities
            .AsNoTracking()
            .ProjectTo<CampusResponse>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == get.Id, cancellationToken);

        if (resultInDb == null)
            return new ResponseWrapper<CampusResponse>().Failed(message: "Campus does not exists.");

        return new ResponseWrapper<CampusResponse>().Success(resultInDb);

    }
}
