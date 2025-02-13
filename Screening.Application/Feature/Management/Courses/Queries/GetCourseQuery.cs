using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Screening.Application.IRepositories;
using Screening.Application.Operations;
using Screening.Common.Models.Courses;
using Screening.Common.Wrapper;
using Screening.Domain.Entities;

namespace Screening.Application.Feature.Management.Courses.Queries;
public class GetCourseQuery : BaseGetQuery<CourseResponse>
{
    public GetCourseQuery(int id) : base(id) { }
}

public class GetCourseQueryHandler : BaseGetQueryHandler<GetCourseQuery, CourseResponse>
{
    public GetCourseQueryHandler(IUnitOfWork<int> unitOfWork, IMapper mapper) : base(unitOfWork, mapper) { }

    public override async Task<ResponseWrapper<CourseResponse>> Handle(GetCourseQuery get, CancellationToken cancellationToken)
    {
        var resultInDb = await _unitOfWork.ReadRepositoryFor<Course>().Entities
               .AsNoTracking()
               .ProjectTo<CourseResponse>(_mapper.ConfigurationProvider)
               .SingleOrDefaultAsync(x => x.Id == get.Id, cancellationToken);

        if (resultInDb == null)
            return new ResponseWrapper<CourseResponse>().Failed(message: "Course does not exists.");

        return new ResponseWrapper<CourseResponse>().Success(resultInDb);
    }
}
