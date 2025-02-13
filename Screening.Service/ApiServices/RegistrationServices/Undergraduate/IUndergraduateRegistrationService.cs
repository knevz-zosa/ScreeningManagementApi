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

namespace Screening.Service.ApiServices.RegistrationServices.Undergraduate;
public interface IUndergraduateRegistrationService
{
    Task<ResponseWrapper<int>> Application(ApplicantRequest request);
    Task<ResponseWrapper<int>> FirstApplication(FirstApplicationInfoRequest request);
    Task<ResponseWrapper<int>> PersonalInformation(PersonalInformationRequest request);
    Task<ResponseWrapper<int>> Spouse(SpouseRequest request);
    Task<ResponseWrapper<int>> SoloParent(SoloParentRequest request);
    Task<ResponseWrapper<int>> Academic(AcademicBackgroundRequest request);
    Task<ResponseWrapper<int>> ParentsGuardian(ParentGuardianInformationRequest request);
    Task<ResponseWrapper<int>> Family(FamilyRelationRequest request);
    Task<ResponseWrapper<int>> Psychiatrist(PsychiatristConsultationRequest request);
    Task<ResponseWrapper<int>> Psychologist(PsychologistConsultationRequest request);
    Task<ResponseWrapper<int>> Counselor(CounselorConsultationRequest request);
    Task<ResponseWrapper<int>> PhysicalHealth(PhysicalHealthRequest request);
    Task<ResponseWrapper<int>> Personality(PersonalityProfileRequest request);
    Task<ResponseWrapper<int>> EmergencyContact(EmergencyContactRequest request);
    Task<ResponseWrapper<int>> Registered(RegisteredRequest request);

}
