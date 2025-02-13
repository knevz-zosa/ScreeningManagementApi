using AutoMapper;
using Screening.Common.Models.Academics;
using Screening.Common.Models.Applicants;
using Screening.Common.Models.Campuses;
using Screening.Common.Models.CouncelorConsultations;
using Screening.Common.Models.Courses;
using Screening.Common.Models.Departments;
using Screening.Common.Models.EmergencyContacts;
using Screening.Common.Models.Examinations;
using Screening.Common.Models.Families;
using Screening.Common.Models.FirstApplications;
using Screening.Common.Models.Interviews;
using Screening.Common.Models.ParentsGuardians;
using Screening.Common.Models.PersonalInformations;
using Screening.Common.Models.Personalities;
using Screening.Common.Models.PhysicalHealths;
using Screening.Common.Models.PsychiatristConsultations;
using Screening.Common.Models.PsychologistConsultations;
using Screening.Common.Models.Registered;
using Screening.Common.Models.Schedules;
using Screening.Common.Models.SoloParents;
using Screening.Common.Models.Spouses;
using Screening.Common.Models.Transfers;
using Screening.Domain.Entities;

namespace Screening.Application.Mapping;
public class MappingProfile : Profile
{
	public MappingProfile()
	{
        CreateMap<Campus, CampusResponse>();        
        CreateMap<CampusResponse, CampusUpdate>();
        CreateMap<Department, DepartmentResponse>();        
        CreateMap<DepartmentResponse, DepartmentUpdate>();
        CreateMap<Course, CourseResponse>();       
        CreateMap<CourseResponse, CourseUpdate>();
        CreateMap<Schedule, ScheduleResponse>();      
        CreateMap<Applicant, ApplicantResponse>();   
        CreateMap<ApplicantResponse, ApplicantTransfer>();
        CreateMap<ApplicantResponse, ApplicantUpdateStudentId>();
        CreateMap<ApplicantResponse, ApplicantUpdateGwaStatusTrack>();
        CreateMap<PersonalInformation, PersonalInformationResponse>();
        CreateMap<PersonalInformationResponse, PersonalInformationUpdate>();
        CreateMap<AcademicBackground, AcademicBackgroundResponse>();
        CreateMap<LrnResponse, LrnUpdate>();
        CreateMap<Examination, ExaminationResponse>();
        CreateMap<ExaminationResponse, ExaminationResultUpdate>();
        CreateMap<Interview, InterviewResponse>();
        CreateMap<InterviewResponse, InterviewActiveUpdate>();
        CreateMap<InterviewResponse, InterviewResultUpdate>();
        CreateMap<FirstApplicationInfo, FirstApplicationInfoResponse>();
        CreateMap<Registered, RegisteredResponse>();
        CreateMap<Spouse, SpouseResponse>();
        CreateMap<PhysicalHealth, PhysicalHealthResponse>();
        CreateMap<CouncelorConsultation, CouncelorConsultationResponse>();
        CreateMap<PsychiatristConsultation, PsychiatristConsultationResponse>();
        CreateMap<PsychologistConsultation, PsychologistConsultationResponse>();
        CreateMap<FamilyRelation, FamilyRelationResponse>();
        CreateMap<ParentGuardianInformation, ParentGuardianInformationResponse>();
        CreateMap<EmergencyContact, EmergencyContactResponse>();
        CreateMap<PersonalityProfile, PersonalityProfileResponse>();
        CreateMap<Transfer, TransferResponse>();
        CreateMap<SoloParent, SoloParentResponse>();
    }
}
