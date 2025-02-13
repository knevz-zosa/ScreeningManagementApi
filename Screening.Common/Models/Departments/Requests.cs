using Screening.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.Departments;
public class DepartmentRequest
{
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }

    [IdValidator(ErrorMessage = "Campus is required")]
    public int CampusId { get; set; }
    public string CreatedBy { get; set; }
}
