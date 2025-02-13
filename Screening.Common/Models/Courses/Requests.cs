using Screening.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.Courses;
public class CourseRequest
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    public int? DepartmentId { get; set; }
    [IdValidator(ErrorMessage = "Campus is required")]
    public int CampusId { get; set; }
    public bool IsOpen { get; set; } = true;

    public string CreatedBy { get; set; }
}
