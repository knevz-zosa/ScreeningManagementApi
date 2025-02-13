using Screening.Common.Extensions;
using Screening.Common.Models.Schedules;
using Screening.Common.Wrapper;

namespace Screening.Service.ApiServices.ScheduleServices;
public interface IScheduleService
{
    Task<ResponseWrapper<int>> Create(ScheduleRequest request);
    Task<ResponseWrapper<int>> Delete(int id);
    Task<ResponseWrapper<ScheduleResponse>> Get(int id);
    Task<ResponseWrapper<PagedList<ScheduleResponse>>> List(DataGridQuery query, string access);
    Task<ResponseWrapper<List<string>>> SchoolYears();
}
