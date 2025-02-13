﻿namespace Screening.Common.Models.PsychologistConsultations;
public class PsychologistConsultationResponse
{
    public int Id { get; set; }
    public DateTime? Start { get; set; }
    public DateTime? End { get; set; }
    public int? Sessions { get; set; }
    public string? Reasons { get; set; }
}

