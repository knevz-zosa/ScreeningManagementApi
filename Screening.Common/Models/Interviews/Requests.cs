using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.Interviews;
public class InterviewResultRequest
{
    public DateTime InterviewDate { get; set; }
    public int CourseId { get; set; }
    public int InterviewReading { get; set; }

    public int InterviewCommunication { get; set; }

    public int InterviewAnalytical { get; set; }
    public int ApplicantId { get; set; }
    public bool IsUse { get; set; } = false;
    public DateTime DateRecorded { get; set; } = DateTime.Now;
    public string RecordedBy { get; set; }
    [Required(ErrorMessage = "Interview name is required")]

    public string Interviewer { get; set; }
}