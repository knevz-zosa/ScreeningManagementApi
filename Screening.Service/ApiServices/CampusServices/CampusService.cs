using Microsoft.AspNetCore.WebUtilities;
using Screening.Common.Extensions;
using Screening.Common.Models.Campuses;
using Screening.Common.Wrapper;
using Screening.Service.Extensions;
using System.Net.Http.Json;

namespace Screening.Service.ApiServices.CampusServices;
public class CampusService : ICampusService
{
    private readonly HttpClient _httpClient;
    public CampusService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseWrapper<int>> Create(CampusRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(CampusEndpoints.Create, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"{CampusEndpoints.Delete}/{id}");
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<CampusResponse>> Get(int id)
    {
        var response = await _httpClient.GetAsync(CampusEndpoints.Get(id));
        return await response.ToResponse<CampusResponse>();
    }

    public async Task<ResponseWrapper<PagedList<CampusResponse>>> List(DataGridQuery query)
    {
        var queryParams = new Dictionary<string, string>();

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

        var url = QueryHelpers.AddQueryString(CampusEndpoints.List, queryParams);

        var response = await _httpClient.GetAsync(url);
        return await response.ToResponse<PagedList<CampusResponse>>();
    }

    public async Task<ResponseWrapper<int>> Update(CampusUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(CampusEndpoints.Update, update);
        return await response.ToResponse<int>();
    }
}
