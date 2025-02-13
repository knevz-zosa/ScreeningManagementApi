using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Schedules;
using Screening.Common.Wrapper;

namespace Screening.Application.Feature.Management.Schedules.Queries;
public class GetScheduleQuery : BaseGetQuery<ScheduleResponse>
{
    public GetScheduleQuery(int id) : base(id) { }
}

public class GetScheduleQueryHandler : BaseGetQueryHandler<GetScheduleQuery, ScheduleResponse>
{
    public GetScheduleQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }
    public override async Task<ResponseWrapper<ScheduleResponse>> Handle(GetScheduleQuery get, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<Schedule>().Entities
            .AsNoTracking()
            .ProjectTo<ScheduleResponse>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync(x => x.Id == get.Id, cancellationToken);

        if (resultInDb == null)
        {
            return new ResponseWrapper<ScheduleResponse>().Failed(message: "Schedule does not exists.");
        }

        return new ResponseWrapper<ScheduleResponse>().Success(resultInDb);
    }
}

