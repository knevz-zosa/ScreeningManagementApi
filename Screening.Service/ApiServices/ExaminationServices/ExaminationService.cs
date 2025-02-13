using Screening.Common.Models.Departments;
using Screening.Common.Models.Examinations;
using Screening.Common.Wrapper;
using Screening.Service.Extensions;
using System.Net.Http.Json;

namespace Screening.Service.ApiServices.ExaminationServices;
public class ExaminationService : IExaminationService
{
    private readonly HttpClient _httpClient;
    public ExaminationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseWrapper<int>> Create(ExaminationResultRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(ExaminationEndpoints.Create, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<ExaminationResponse>> Get(int id)
    {
        var response = await _httpClient.GetAsync(ExaminationEndpoints.Get(id));
        return await response.ToResponse<ExaminationResponse>();
    }

    public async Task<ResponseWrapper<int>> Update(ExaminationResultUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(ExaminationEndpoints.Update, update);
        return await response.ToResponse<int>();
    }
}
