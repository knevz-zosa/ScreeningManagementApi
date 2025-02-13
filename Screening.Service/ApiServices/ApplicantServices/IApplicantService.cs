using Screening.Common.Extensions;
using Screening.Common.Models.Academics;
using Screening.Common.Models.Applicants;
using Screening.Common.Models.EmergencyContacts;
using Screening.Common.Models.FirstApplications;
using Screening.Common.Models.PersonalInformations;
using Screening.Common.Wrapper;

namespace Screening.Service.ApiServices.ApplicantServices;
public interface IApplicantService
{
    Task<ResponseWrapper<int>> Transfer(ApplicantRequest request);
    Task<ResponseWrapper<int>> Delete(int id);
    Task<ResponseWrapper<int>> UpdateEmergencyContact(EmergencyContactUpdate update);
    Task<ResponseWrapper<int>> UpdateGWAStatusTrack(ApplicantUpdateGwaStatusTrack update);
    Task<ResponseWrapper<int>> UpdateLRN(LrnUpdate update);
    Task<ResponseWrapper<int>> UpdatePersonalInformation(PersonalInformationUpdate update);
    Task<ResponseWrapper<int>> UpdateTransfer(ApplicantTransfer update);    
    Task<ResponseWrapper<int>> UpdateStudentId(ApplicantUpdateStudentId update); 
    Task<ResponseWrapper<ApplicantResponse>> Get(int id);
    Task<ResponseWrapper<FirstApplicationInfoResponse>> GetFirstApplication(int id);
    Task<ResponseWrapper<PersonalInformationResponse>> GetPersonalInformation(int id);
    Task<ResponseWrapper<LrnResponse>> GetLRN(int id);
    Task<ResponseWrapper<PagedList<ApplicantResponse>>> List(DataGridQuery query, int? Id, string access);
    Task<ResponseWrapper<PagedList<ApplicantResponse>>> InProgress(DataGridQuery query, string access);
    Task<ResponseWrapper<PagedList<ApplicantResponse>>> Passers(DataGridQuery query, string access);
}
