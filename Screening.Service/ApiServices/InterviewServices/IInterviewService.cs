using Screening.Common.Extensions;
using Screening.Common.Models.Interviews;
using Screening.Common.Wrapper;

namespace Screening.Service.ApiServices.InterviewServices;
public interface IInterviewService
{
    Task<ResponseWrapper<int>> Create(InterviewResultRequest request);
    Task<ResponseWrapper<int>> UpdateRating(InterviewResultUpdate update);
    Task<ResponseWrapper<int>> Activate(InterviewActiveUpdate update);
    Task<ResponseWrapper<InterviewResponse>> Get(int id);
    Task<ResponseWrapper<PagedList<InterviewResponse>>> List(DataGridQuery query);

}
