using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.Academics;
public class LrnUpdate
{
    public int Id { get; set; }
    public int ApplicantId { get; set; }
    [Required(ErrorMessage = "LRN is required")]
    [StringLength(12, MinimumLength = 12, ErrorMessage = "LRN must be exactly 12 digits")]
    public string LRN { get; set; }
}
