using Screening.Service.ApiServices.ApplicantServices;
using Screening.Service.ApiServices.AuthServices;
using Screening.Service.ApiServices.CampusServices;
using Screening.Service.ApiServices.CourceServices;
using Screening.Service.ApiServices.DepartmentServices;
using Screening.Service.ApiServices.ExaminationServices;
using Screening.Service.ApiServices.InterviewServices;
using Screening.Service.ApiServices.RegistrationServices.Undergraduate;
using Screening.Service.ApiServices.ScheduleServices;
using Screening.Service.ApiServices.UserServices;

namespace Screening.Service.ApiServices._Connect;
public class ConnectService : IConnectService
{
    public IAuthService Auth { get; }
    public IUserService User { get; }    
    public ICampusService Campus { get; }
    public IDepartmentService Department { get; }
    public ICourseService Course { get; }
    public IScheduleService Schedule { get; }
    public IExaminationService Examination { get; }
    public IInterviewService Interview { get; }
    public IApplicantService Applicant { get; }
    public IUndergraduateRegistrationService Undergraduate { get; }
    public ConnectService
    (
        IAuthService auth,
        IUserService user,       
        ICampusService campus,
        IDepartmentService department,
        ICourseService course,
        IScheduleService schedule,
        IExaminationService examination,
        IInterviewService interview,
        IApplicantService applicant,
        IUndergraduateRegistrationService undergraduate
    )
    {
        Auth = auth;
        User = user;        
        Campus = campus;
        Department = department;
        Course = course;
        Schedule = schedule;
        Examination = examination;
        Interview = interview;
        Applicant = applicant;
        Undergraduate = undergraduate;
    }
}
