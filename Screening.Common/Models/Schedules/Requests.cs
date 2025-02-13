using Screening.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.Schedules;
public class ScheduleRequest
{
    [Required(ErrorMessage = "Schedule date is required")]
    public DateTime ScheduleDate { get; set; }
    [Required(ErrorMessage = "Venue is required")]
    public string Venue { get; set; }
    [Required(ErrorMessage = "Time year is required")]
    public string Time { get; set; }
    public int Slot { get; set; }
    [IdValidator(ErrorMessage = "Campus is required")]
    public int CampusId { get; set; }
    public DateTime DateCreated { get; set; } = DateTime.Now;
    public string CreatedBy { get; set; }
}
