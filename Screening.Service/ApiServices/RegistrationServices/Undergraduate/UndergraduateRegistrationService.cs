using Screening.Common.Models.Academics;
using Screening.Common.Models.Applicants;
using Screening.Common.Models.CouncelorConsultations;
using Screening.Common.Models.EmergencyContacts;
using Screening.Common.Models.Families;
using Screening.Common.Models.FirstApplications;
using Screening.Common.Models.ParentsGuardians;
using Screening.Common.Models.PersonalInformations;
using Screening.Common.Models.Personalities;
using Screening.Common.Models.PhysicalHealths;
using Screening.Common.Models.PsychiatristConsultations;
using Screening.Common.Models.PsychologistConsultations;
using Screening.Common.Models.Registered;
using Screening.Common.Models.SoloParents;
using Screening.Common.Models.Spouses;
using Screening.Common.Wrapper;
using Screening.Service.Extensions;
using System.Net.Http.Json;

namespace Screening.Service.ApiServices.RegistrationServices.Undergraduate;
public class UndergraduateRegistrationService : IUndergraduateRegistrationService
{
    private readonly HttpClient _httpClient;
    public UndergraduateRegistrationService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseWrapper<int>> Academic(AcademicBackgroundRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.Academic, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> Application(ApplicantRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.Application , request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> Counselor(CounselorConsultationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.Counselor, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> EmergencyContact(EmergencyContactRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.EmergencyContact, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> Family(FamilyRelationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.Family, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> FirstApplication(FirstApplicationInfoRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.FirstApplication, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> ParentsGuardian(ParentGuardianInformationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.ParentsGuardian, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> PersonalInformation(PersonalInformationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.PersonalInformation, request);
        return await response.ToResponse<int>(); ;
    }

    public async Task<ResponseWrapper<int>> Personality(PersonalityProfileRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.Personality, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> PhysicalHealth(PhysicalHealthRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.PhysicalHealth, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> Psychiatrist(PsychiatristConsultationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.Psychiatrist, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> Psychologist(PsychologistConsultationRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.Psychologist, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> Registered(RegisteredRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.Registered, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> SoloParent(SoloParentRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.SoloParent, request);
        return await response.ToResponse<int>();
    }

    public async Task<ResponseWrapper<int>> Spouse(SpouseRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync(UndergraduateEndpoints.Spouse, request);
        return await response.ToResponse<int>();
    }
}
