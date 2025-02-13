﻿using Screening.Domain.Contracts;

namespace Screening.Domain.Entities;
public class ParentGuardianInformation : BaseEntity<int>
{
    public int ApplicantId { get; set; }
    public Applicant Applicant { get; set; }
    public string FatherFirstName { get; set; }
    public string? FatherMiddleName { get; set; }
    public string FatherLastName { get; set; }
    public string? FatherContactNo { get; set; }
    public string FatherCitizenship { get; set; }
    public string? FatherEmail { get; set; }
    public string? FatherOccupation { get; set; }
    public DateTime FatherBirthday { get; set; }
    public string FatherBirthPlace { get; set; }
    public string FatherReligion { get; set; }
    public string FatherMaritalStatus { get; set; }
    public string FatherDialect { get; set; }
    public string FatherPermanentAddress { get; set; }
    public string FatherEducation { get; set; }
    public string FatherEstimatedMonthly { get; set; }
    public string FatherOtherIncome { get; set; }
    public string MotherFirstName { get; set; }
    public string? MotherMiddleName { get; set; }
    public string MotherLastName { get; set; }
    public string? MotherContactNo { get; set; }
    public string MotherCitizenship { get; set; }
    public string? MotherEmail { get; set; }
    public string? MotherOccupation { get; set; }
    public DateTime MotherBirthday { get; set; }
    public string MotherBirthPlace { get; set; }
    public string MotherReligion { get; set; }
    public string MotherMaritalStatus { get; set; }
    public string MotherDialect { get; set; }
    public string MotherPermanentAddress { get; set; }
    public string MotherEducation { get; set; }
    public string MotherEstimatedMonthly { get; set; }
    public string MotherOtherIncome { get; set; }
    public string GuardianFirstName { get; set; }
    public string? GuardianMiddleName { get; set; }
    public string GuardianLastName { get; set; }
    public string? GuardianContactNo { get; set; }
    public string GuardianCitizenship { get; set; }
    public string? GuardianEmail { get; set; }
    public string? GuardianOccupation { get; set; }
    public DateTime GuardianBirthday { get; set; }
    public string GuardianBirthPlace { get; set; }
    public string GuardianReligion { get; set; }
    public string GuardianMaritalStatus { get; set; }
    public string GuardianDialect { get; set; }
    public string GuardianPermanentAddress { get; set; }
    public string GuardianEducation { get; set; }
    public string GuardianEstimatedMonthly { get; set; }
    public string GuardianOtherIncome { get; set; }
}

