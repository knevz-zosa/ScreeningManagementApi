namespace Screening.Common.Models.Registered;
public class RegisteredRequest
{
    public int ApplicantId { get; set; }
    public DateTime RegistrationDate { get; set; } = DateTime.Now;
}
