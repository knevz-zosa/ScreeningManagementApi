using Screening.Common.Models.Examinations;
using Screening.Common.Wrapper;

namespace Screening.Service.ApiServices.ExaminationServices;
public interface IExaminationService
{
    Task<ResponseWrapper<int>> Create(ExaminationResultRequest request);
    Task<ResponseWrapper<int>> Update(ExaminationResultUpdate update);
    Task<ResponseWrapper<ExaminationResponse>> Get(int id);
}
