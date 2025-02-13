﻿using Screening.Common.Extensions;
using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.Courses;
public class CourseUpdate
{
    public int Id { get; set; }
    [Required(ErrorMessage = "Name is required")]
    public string Name { get; set; }
    public int? DepartmentId { get; set; }
    [IdValidator(ErrorMessage = "Campus is required")]
    public int CampusId { get; set; }
    public bool IsOpen { get; set; } = false;
    public string UpdatedBy { get; set; }
}
