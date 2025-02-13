using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.Campuses;
public class CampusRequest
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Address is required")]
    public string Address { get; set; }
    public bool HasDepartment { get; set; } = false;
    public string CreatedBy { get; set; }

}

