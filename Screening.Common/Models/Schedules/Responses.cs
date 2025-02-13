using Screening.Common.Models.Applicants;
using Screening.Common.Models.Campuses;

namespace Screening.Common.Models.Schedules;
public class ScheduleResponse
{
    public int Id { get; set; }
    public DateTime ScheduleDate { get; set; }
    public string SchoolYear { get; set; }
    public string Venue { get; set; }
    public string Time { get; set; }
    public int Slot { get; set; }
    public int CampusId { get; set; }
    public CampusResponse Campus { get; set; }
    public List<ApplicantResponse> Applicants { get; set; }
    public DateTime DateCreated { get; set; }
    public string CreatedBy { get; set; }
    public int Available => AvailableSlots();

    private int AvailableSlots()
    {
        if (Applicants == null || Applicants.Count == 0)
        {
            return Slot;
        }
        else
        {
            int completedApplicantsCount = Applicants.Count(x => x.Registered != null);

            int availableSlots = Slot - completedApplicantsCount;

            return Math.Max(availableSlots, 0);
        }
    }
}
