namespace Screening.Common.Models.Applicants;
public class ApplicantTransfer
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public int ScheduleId { get; set; }
}

public class ApplicantUpdateGwaStatusTrack
{
    public int Id { get; set; }
    public double GWA { get; set; }
    public string ApplicantStatus { get; set; }
    public string Track { get; set; }
}

public class ApplicantUpdateStudentId
{
    public int Id { get; set; }
    public string? StudentId { get; set; }
}

