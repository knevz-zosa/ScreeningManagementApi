using Screening.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.Departments;
public class DepartmentUpdate
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is require.")]
    public string Name { get; set; }
    [IdValidator(ErrorMessage = "Campus is required")]
    public int CampusId { get; set; }
    public string UpdatedBy { get; set; }
}