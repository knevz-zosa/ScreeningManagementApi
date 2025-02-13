using Screening.Domain.Contracts;

namespace Screening.Domain.Entities;
public class CouncelorConsultation : BaseEntity<int>
{
    public int ApplicantId { get; set; }
    public Applicant Applicant { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public int? Sessions { get; set; }
    public string? Reasons { get; set; }
}

