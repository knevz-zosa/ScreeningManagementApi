using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.Interviews;
public class InterviewResultUpdate
{
    public int Id { get; set; }
    public int InterviewReading { get; set; }

    public int InterviewCommunication { get; set; }

    public int InterviewAnalytical { get; set; }
    public string UpdatedBy { get; set; }
    [Required(ErrorMessage = "Interview name is required")]
    public string Interviewer { get; set; }
}

public class InterviewActiveUpdate
{
    public int Id { get; set; }
    public bool IsUse { get; set; }
    public string UpdatedBy { get; set; }
}