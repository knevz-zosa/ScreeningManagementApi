using Screening.Common.Extensions;
using Screening.Common.Models.Departments;
using Screening.Common.Wrapper;

namespace Screening.Service.ApiServices.DepartmentServices;
public interface IDepartmentService
{
    Task<ResponseWrapper<int>> Create(DepartmentRequest request);
    Task<ResponseWrapper<int>> Update(DepartmentUpdate update);
    Task<ResponseWrapper<int>> Delete(int id);
    Task<ResponseWrapper<DepartmentResponse>> Get(int id);
    Task<ResponseWrapper<PagedList<DepartmentResponse>>> List(DataGridQuery query);
}
