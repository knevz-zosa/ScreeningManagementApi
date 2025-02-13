using Microsoft.AspNetCore.WebUtilities;
using Screening.Common.Extensions;
using Screening.Common.Models.Academics;
using Screening.Common.Models.Applicants;
using Screening.Common.Models.Courses;
using Screening.Common.Models.EmergencyContacts;
using Screening.Common.Models.FirstApplications;
using Screening.Common.Models.PersonalInformations;
using Screening.Common.Wrapper;
using Screening.Service.Extensions;
using System.Net.Http.Json;

namespace Screening.Service.ApiServices.ApplicantServices;
public class ApplicantService : IApplicantService
{
    private readonly HttpClient _httpClient;
    public ApplicantService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseWrapper<int>> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"{ApplicantEndpoints.Delete}/{id}");
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<ApplicantResponse>> Get(int id)
    {
        var response = await _httpClient.GetAsync(ApplicantEndpoints.Get(id));
        return await response.ToResponse<ApplicantResponse>();
    }

    public async Task<ResponseWrapper<FirstApplicationInfoResponse>> GetFirstApplication(int id)
    {
        var response = await _httpClient.GetAsync(ApplicantEndpoints.GetFirstApplication(id));
        return await response.ToResponse<FirstApplicationInfoResponse>();
    }

    public async Task<ResponseWrapper<LrnResponse>> GetLRN(int id)
    {
        var response = await _httpClient.GetAsync(ApplicantEndpoints.GetLRN(id));
        return await response.ToResponse<LrnResponse>();
    }

    public async Task<ResponseWrapper<PersonalInformationResponse>> GetPersonalInformation(int id)
    {
        var response = await _httpClient.GetAsync(ApplicantEndpoints.GetPersonalInformation(id));
        return await response.ToResponse<PersonalInformationResponse>();
    }

    public async Task<ResponseWrapper<PagedList<ApplicantResponse>>> InProgress(DataGridQuery query, string access)
    {
        var queryParams = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(access))
            queryParams["access"] = access;

        if (!string.IsNullOrWhiteSpace(query.Search))
            queryParams["search"] = query.Search;

        if (!string.IsNullOrWhiteSpace(query.SchoolYear))
            queryParams["schoolYear"] = query.SchoolYear;

        if (query.Page.HasValue)
            queryParams["page"] = query.Page.Value.ToString();

        if (query.PageSize.HasValue)
            queryParams["pageSize"] = query.PageSize.Value.ToString();

        if (!string.IsNullOrWhiteSpace(query.SortField))
            queryParams["sortField"] = query.SortField;

        queryParams["sortDir"] = query.SortDir.ToString().ToLower();

        var url = QueryHelpers.AddQueryString(ApplicantEndpoints.InProgress, queryParams);

        var response = await _httpClient.GetAsync(url);
        return await response.ToResponse<PagedList<ApplicantResponse>>();
    }

    public async Task<ResponseWrapper<PagedList<ApplicantResponse>>> List(DataGridQuery query, int? Id, string access)
    {
        var queryParams = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(access))
            queryParams["scheduleid"] = Id.ToString() ?? string.Empty;

        if (!string.IsNullOrWhiteSpace(access))
            queryParams["access"] = access;

        if (!string.IsNullOrWhiteSpace(query.Search))
            queryParams["search"] = query.Search;

        if (!string.IsNullOrWhiteSpace(query.SchoolYear))
            queryParams["schoolYear"] = query.SchoolYear;

        if (query.Page.HasValue)
            queryParams["page"] = query.Page.Value.ToString();

        if (query.PageSize.HasValue)
            queryParams["pageSize"] = query.PageSize.Value.ToString();

        if (!string.IsNullOrWhiteSpace(query.SortField))
            queryParams["sortField"] = query.SortField;

        queryParams["sortDir"] = query.SortDir.ToString().ToLower();

        var url = QueryHelpers.AddQueryString(ApplicantEndpoints.List, queryParams);

        var response = await _httpClient.GetAsync(url);
        return await response.ToResponse<PagedList<ApplicantResponse>>();
    }

    public async Task<ResponseWrapper<PagedList<ApplicantResponse>>> Passers(DataGridQuery query, string access)
    {
        var queryParams = new Dictionary<string, string>();

        if (!string.IsNullOrWhiteSpace(access))
            queryParams["access"] = access;

        if (!string.IsNullOrWhiteSpace(query.Search))
            queryParams["search"] = query.Search;

        if (!string.IsNullOrWhiteSpace(query.SchoolYear))
            queryParams["schoolYear"] = query.SchoolYear;

        if (query.Page.HasValue)
            queryParams["page"] = query.Page.Value.ToString();

        if (query.PageSize.HasValue)
            queryParams["pageSize"] = query.PageSize.Value.ToString();

        if (!string.IsNullOrWhiteSpace(query.SortField))
            queryParams["sortField"] = query.SortField;

        queryParams["sortDir"] = query.SortDir.ToString().ToLower();

        var url = QueryHelpers.AddQueryString(ApplicantEndpoints.Passers, queryParams);

        var response = await _httpClient.GetAsync(url);
        return await response.ToResponse<PagedList<ApplicantResponse>>();
    }

    public async Task<ResponseWrapper<int>> Transfer(ApplicantRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(ApplicantEndpoints.Transfer, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> UpdateEmergencyContact(EmergencyContactUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(ApplicantEndpoints.UpdateEmergencyContact, update);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> UpdateGWAStatusTrack(ApplicantUpdateGwaStatusTrack update)
    {
        var response = await _httpClient.PutAsJsonAsync(ApplicantEndpoints.UpdateGwaStatusTrack, update);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> UpdateLRN(LrnUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(ApplicantEndpoints.UpdateLRN, update);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> UpdatePersonalInformation(PersonalInformationUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(ApplicantEndpoints.UpdatePersonalInformation, update);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> UpdateStudentId(ApplicantUpdateStudentId update)
    {
        var response = await _httpClient.PutAsJsonAsync(ApplicantEndpoints.UpdateStudentId, update);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> UpdateTransfer(ApplicantTransfer update)
    {
        var response = await _httpClient.PutAsJsonAsync(ApplicantEndpoints.UpdateTransfer, update);
        return await response.ToResponse<int>();
    }
}
