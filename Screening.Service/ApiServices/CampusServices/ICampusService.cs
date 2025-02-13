using Screening.Common.Extensions;
using Screening.Common.Models.Campuses;
using Screening.Common.Wrapper;

namespace Screening.Service.ApiServices.CampusServices;
public interface ICampusService
{
    Task<ResponseWrapper<int>> Create(CampusRequest request);
    Task<ResponseWrapper<int>> Update(CampusUpdate update);
    Task<ResponseWrapper<int>> Delete(int id);
    Task<ResponseWrapper<CampusResponse>> Get(int id);
    Task<ResponseWrapper<PagedList<CampusResponse>>> List(DataGridQuery query);
}

