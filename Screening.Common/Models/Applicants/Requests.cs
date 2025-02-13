using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.Applicants;
public class ApplicantRequest
{
    public int CourseId { get; set; }
    public int ScheduleId { get; set; }
    [Required(ErrorMessage = "Applicant status is required.")]
    public string ApplicantStatus { get; set; }
    [Required(ErrorMessage = "Track is required.")]
    public string Track { get; set; }
    [Required(ErrorMessage = "Time is required.")]
    public DateTime TransactionDate { get; set; } = DateTime.Now;
}

