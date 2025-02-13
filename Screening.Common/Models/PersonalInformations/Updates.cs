using System.ComponentModel.DataAnnotations;

namespace Screening.Common.Models.PersonalInformations;
public class PersonalInformationUpdate
{
    public int Id { get; set; }
    public int ApplicantId { get; set; }
    [Required(ErrorMessage = "First name  is required.")]
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    [Required(ErrorMessage = "Last name is required.")]
    public string LastName { get; set; }
    public string? NameExtension { get; set; }
    [Required(ErrorMessage = "Nick name is required.")]
    public string NickName { get; set; }
    public string Sex { get; set; }
    public DateTime DateofBirth { get; set; }
    [Required(ErrorMessage = "Place of birth is required.")]
    public string PlaceOfBirth { get; set; }
    [Required(ErrorMessage = "Citizenship is required.")]
    public string Citizenship { get; set; }
    public string? Email { get; set; }
    public string? ContactNumber { get; set; }
    public string? HouseNo { get; set; }
    [Required(ErrorMessage = "Street address is required.")]
    public string Street { get; set; }
    [Required(ErrorMessage = "Barangay address is required.")]
    public string Barangay { get; set; }
    public string? Purok { get; set; }
    [Required(ErrorMessage = "Municipality address is required.")]
    public string Municipality { get; set; }
    [Required(ErrorMessage = "Province address is required.")]
    public string Province { get; set; }
    [Required(ErrorMessage = "Zip code is required.")]
    public string ZipCode { get; set; }
}

