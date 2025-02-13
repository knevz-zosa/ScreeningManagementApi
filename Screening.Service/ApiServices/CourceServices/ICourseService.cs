using Screening.Common.Extensions;
using Screening.Common.Models.Courses;
using Screening.Common.Wrapper;

namespace Screening.Service.ApiServices.CourceServices;
public interface ICourseService
{
    Task<ResponseWrapper<int>> Create(CourseRequest request);
    Task<ResponseWrapper<int>> Update(CourseUpdate update);
    Task<ResponseWrapper<int>> Delete(int id);
    Task<ResponseWrapper<CourseResponse>> Get(int id);
    Task<ResponseWrapper<PagedList<CourseResponse>>> List(DataGridQuery query, string access);
}
