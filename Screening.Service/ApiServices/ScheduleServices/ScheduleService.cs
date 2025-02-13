using Microsoft.AspNetCore.WebUtilities;
using Screening.Common.Extensions;
using Screening.Common.Models.Departments;
using Screening.Common.Models.Schedules;
using Screening.Common.Wrapper;
using Screening.Service.Extensions;
using System.Net.Http.Json;

namespace Screening.Service.ApiServices.ScheduleServices;
public class ScheduleService : IScheduleService
{
    private readonly HttpClient _httpClient;
    public ScheduleService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseWrapper<int>> Create(ScheduleRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(ScheduleEndpoints.Create, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"{ScheduleEndpoints.Delete}/{id}");
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<ScheduleResponse>> Get(int id)
    {
        var response = await _httpClient.GetAsync(ScheduleEndpoints.Get(id));
        return await response.ToResponse<ScheduleResponse>();
    }

    public async Task<ResponseWrapper<PagedList<ScheduleResponse>>> List(DataGridQuery query, string access)
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

        var url = QueryHelpers.AddQueryString(ScheduleEndpoints.List, queryParams);

        var response = await _httpClient.GetAsync(url);
        return await response.ToResponse<PagedList<ScheduleResponse>>();
    }

    public async Task<ResponseWrapper<List<string>>> SchoolYears()
    {
        var response = await _httpClient.GetAsync(ScheduleEndpoints.SchoolYears);
        return await response.ToResponse<List<string>>();
    }
}
