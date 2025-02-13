using Microsoft.AspNetCore.WebUtilities;
using Screening.Common.Extensions;
using Screening.Common.Models.Courses;
using Screening.Common.Wrapper;
using Screening.Service.Extensions;
using System.Net.Http.Json;

namespace Screening.Service.ApiServices.CourceServices;
public class CourseService : ICourseService
{
    private readonly HttpClient _httpClient;
    public CourseService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseWrapper<int>> Create(CourseRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(CourseEndpoints.Create, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"{CourseEndpoints.Delete}/{id}");
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<CourseResponse>> Get(int id)
    {
        var response = await _httpClient.GetAsync(CourseEndpoints.Get(id));
        return await response.ToResponse<CourseResponse>();
    }

    public async Task<ResponseWrapper<PagedList<CourseResponse>>> List(DataGridQuery query, string access)
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

        var url = QueryHelpers.AddQueryString(CourseEndpoints.List, queryParams);

        var response = await _httpClient.GetAsync(url);
        return await response.ToResponse<PagedList<CourseResponse>>();
    }

    public async Task<ResponseWrapper<int>> Update(CourseUpdate update)
    {
        var response = await _httpClient.PutAsJsonAsync(CourseEndpoints.Update, update);
        return await response.ToResponse<int>();
    }
}
