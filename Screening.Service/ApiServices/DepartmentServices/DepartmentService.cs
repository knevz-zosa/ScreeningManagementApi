using Microsoft.AspNetCore.WebUtilities;
using Screening.Common.Extensions;
using Screening.Common.Models.Departments;
using Screening.Common.Wrapper;
using Screening.Service.Extensions;
using System.Net.Http.Json;

namespace Screening.Service.ApiServices.DepartmentServices;
public class DepartmentService : IDepartmentService
{
    private readonly HttpClient _httpClient;
    public DepartmentService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseWrapper<int>> Create(DepartmentRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(DepartmentEndpoints.Create, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"{DepartmentEndpoints.Delete}/{id}");
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<DepartmentResponse>> Get(int id)
    {
        var response = await _httpClient.GetAsync(DepartmentEndpoints.Get(id));
        return await response.ToResponse<DepartmentResponse>();
    }

    public async Task<ResponseWrapper<PagedList<DepartmentResponse>>> List(DataGridQuery query)
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

        var url = QueryHelpers.AddQueryString(DepartmentEndpoints.List, queryParams);

        var response = await _httpClient.GetAsync(url);
        return await response.ToResponse<PagedList<DepartmentResponse>>();
    }

    public async Task<ResponseWrapper<int>> Update(DepartmentUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(DepartmentEndpoints.Update, update);
        return await response.ToResponse<int>();
    }
}
