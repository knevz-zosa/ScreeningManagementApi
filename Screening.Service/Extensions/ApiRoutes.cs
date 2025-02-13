namespace Screening.Service.Extensions;
public static class UserEndpoints
{
    public const string Create = "api/users/register";
    public const string UpdateProfile = "api/users/update-profile";
    public const string UpdateUsername = "api/users/update-username";
    public const string UpdatePassword = "api/users/update-password";
    public const string UpdateRole = "api/users/update-role";
    public const string UpdateAccess = "api/users/update-access";
    public const string UpdateIsActive = "api/users/update-status";
    public const string Delete = "api/users";
    public const string List = "api/users/list";
    public const string Roles = "api/users/roles";

    public static string Get(int id)
    {
        return $"api/users/{id}";
    }
}

public static class AuthEndpoints
{
    public const string Login = "api/auth/login";
    public const string RefreshToken = "api/auth/refresh-token";
    public const string RemoveRefreshToken = "api/auth/remove-refresh-token";
    public const string CurrentUser = "api/auth/current-user";
    public const string Logout = "api/auth/logout";
}

public static class CampusEndpoints
{
    public const string Create = "api/campuses/add";
    public const string Update = "api/campuses/update";
    public const string Delete = "api/campuses";
    public const string List = "api/campuses/list";

    public static string Get(int id)
    {
        return $"api/campuses/{id}";
    }
}
public static class DepartmentEndpoints
{
    public const string Create = "api/departments/add";
    public const string Update = "api/departments/update";
    public const string Delete = "api/departments";
    public const string List = "api/departments/list";

    public static string Get(int id)
    {
        return $"api/departments/{id}";
    }
}

public static class CourseEndpoints
{
    public const string Create = "api/courses/add";
    public const string Update = "api/courses/update";
    public const string Delete = "api/courses";
    public const string List = "api/courses/list";

    public static string Get(int id)
    {
        return $"api/courses/{id}";
    }
}

public static class ScheduleEndpoints
{
    public const string Create = "api/schedules/add";
    public const string Delete = "api/schedules";
    public const string List = "api/schedules/list";
    public const string SchoolYears = "api/schedules/schoolyears";

    public static string Get(int id)
    {
        return $"api/schedules/{id}";
    }
}
public static class ExaminationEndpoints
{
    public const string Create = "api/examresults/score";
    public const string Update = "api/examresults/update";

    public static string Get(int id)
    {
        return $"api/examresults/{id}";
    }
}

public static class InterviewEndpoints
{
    public const string Create = "api/interviewresults/score";
    public const string Update = "api/interviewresults/update";
    public const string IsActive = "api/interviewresults/isactive";
    public const string List = "api/interviewresults/list";

    public static string Get(int id)
    {
        return $"api/interviewresults/{id}";
    }
}

public static class UndergraduateEndpoints
{
    public const string Application = "api/undergraduates/application";
    public const string Spouse = "api/undergraduates/spouse";
    public const string SoloParent = "api/undergraduates/solo-parent";
    public const string FirstApplication = "api/undergraduates/first-application";
    public const string PersonalInformation = "api/undergraduates/personal-information";
    public const string Academic = "api/undergraduates/academic-background";
    public const string ParentsGuardian = "api/undergraduates/parents-guardian-information";
    public const string Family = "api/undergraduates/family";
    public const string Counselor = "api/undergraduates/counselor-consultation";
    public const string Psychiatrist = "api/undergraduates/psychiatrist-consultation";
    public const string Psychologist = "api/undergraduates/psychologist-consultation";
    public const string PhysicalHealth = "api/undergraduates/physical-health";
    public const string Personality = "api/undergraduates/personality-profile";
    public const string EmergencyContact = "api/undergraduates/emergency-contact";
    public const string Registered = "api/undergraduates/registered"; 
}

public static class ApplicantEndpoints
{
    public const string Transfer = "api/applicants/transfer";
    public const string Delete = "api/applicants";
    public const string UpdateEmergencyContact = "api/applicants/update-emergency-contact";
    public const string UpdateGwaStatusTrack = "api/applicants/update-gwa-status-track";
    public const string UpdateLRN = "api/applicants/update-lrn";
    public const string UpdatePersonalInformation = "api/applicants/update-personal-information";
    public const string UpdateStudentId = "api/applicants/update-studentid";
    public const string UpdateTransfer = "api/applicants/update-transfer";
    public const string List = "api/applicants/list";
    public const string InProgress = "api/applicants/in-progress";
    public const string Passers = "api/applicants/passers";

    public static string Get(int id)
    {
        return $"api/applicants/{id}";
    }
    public static string GetFirstApplication(int id)
    {
        return $"api/applicants/first-application/{id}";
    }
    public static string GetPersonalInformation(int id)
    {
        return $"api/applicants/personalinformation/{id}";
    }
    public static string GetLRN(int id)
    {
        return $"api/applicants/lrn/{id}";
    }
}
