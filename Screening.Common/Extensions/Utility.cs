using Screening.Common.Models.Applicants;
using System.Text.RegularExpressions;

namespace Screening.Common.Extensions;
public static class Utility
{
    public static string GenerateControlNumber()
    {
        return DateTime.Now.ToString("yyMMddHHmmss");
    }
    public static double GWAPercentage(double GWA)
    {
        double result = GWA * 0.20;
        return Math.Round(result, 2);
    }

    public static int Age(DateTime DateOfBirth)
    {
        DateTime today = DateTime.Today;
        int age = today.Year - DateOfBirth.Year;
        if (today < DateOfBirth.AddYears(age))
        {
            age--;
        }
        return age;
    }

    public static string AnnualIncome(string Monthly)
    {
        if (Monthly == "Not Applicable")
            return "Not Applicable";
        else if (Monthly == "Below P10,000")
            return "Below P120,000";
        else if (Monthly == "P10,000 - P20,000")
            return "P120,000 - P240,000";
        else if (Monthly == "P20,000 - P30,000")
            return "P240,000 - P360,000";
        else if (Monthly == "P30,000 - P40,000")
            return "P360,000 - P480,000";
        else if (Monthly == "P40,000 - P50,000")
            return "P480,000 - P600,000";
        else if (Monthly == "Above P50,000")
            return "Above P600,000";
        else return "";
    }

    public static string FullName(ApplicantResponse model)
    {
        var lastName = model.PersonalInformation.LastName;
        var firstName = model.PersonalInformation.FirstName;
        var nameExtension = model.PersonalInformation.NameExtension;

        // Get the first character if MiddleName exists and has at least one character
        var middleNameInitial = model.PersonalInformation.MiddleName?.Length > 0
            ? model.PersonalInformation.MiddleName[0] + "."
            : string.Empty;

        return $"{lastName}, {firstName} {nameExtension} {middleNameInitial}";
    }

    public static DateTime? SoloParentIdExpirationDate(ApplicantResponse model)
    {
        if (model.SoloParent != null)
        {
            if (model.SoloParent.End != null)
            {
                return model.SoloParent.End;
            }
        }
        return null;
    }

    public static DateTime? SoloParentIdDateIssued(ApplicantResponse model)
    {
        if (model.SoloParent != null)
        {
            if (model.SoloParent.Start != null)
            {
                return model.SoloParent.Start;
            }
        }
        return null;
    }

    public static string SoloParentId(ApplicantResponse model)
    {
        if (model.SoloParent != null)
        {
            if (!string.IsNullOrEmpty(model.SoloParent.SoloParentId))
            {
                return model.SoloParent.SoloParentId;
            }
        }
        return "N/A";
    }
    public static string SpouseFullname(ApplicantResponse model)
    {
        if (model.Spouse != null)
        {
            return model.Spouse.FullName;
        }

        else return "N/A";
    }

    public static string SpouseOccupation(ApplicantResponse model)
    {
        if (model.Spouse != null)
        {
            if (!string.IsNullOrEmpty(model.Spouse.Occupation))
            {
                return model.Spouse.Occupation;
            }
        }
        return "N/A";
    }

    public static string SpouseAddress(ApplicantResponse model)
    {
        if (model.Spouse != null)
        {
            return $"{model.Spouse.Municipality} {model.Spouse.Province}";
        }
        return "N/A";
    }

    public static string SpouseContact(ApplicantResponse model)
    {
        if (model.Spouse != null)
        {
            if (!string.IsNullOrEmpty(model.Spouse.ContactNumber))
            {
                return model.Spouse.ContactNumber;
            }
        }
        return "N/A";
    }

    public static string SpouseOfficeAddress(ApplicantResponse model)
    {
        if (model.Spouse != null)
        {
            if (!string.IsNullOrEmpty(model.Spouse.OfficeAddress))
            {
                return model.Spouse.OfficeAddress;
            }
        }
        return "N/A";
    }
    public static string SpouseEducation(ApplicantResponse model)
    {
        if (model.Spouse != null)
        {
            if (!string.IsNullOrEmpty(model.Spouse.Education))
            {
                return model.Spouse.Education;
            }
        }
        return "N/A";
    }

    public static string DepartmentName(ApplicantResponse model)
    {
        if (model.Schedule.Campus.HasDepartment)
        {
            return model.Schedule.Campus.Departments
                .Where(x => x.Courses.Any(x => x.Id == model.CourseId))
                .Select(x => x.Name)
                .FirstOrDefault() ?? "N/A";
        }
        return "N/A";
    }

    public static string CourseName(ApplicantResponse model)
    {
        return model.Schedule.Campus.Courses
           .Where(x => x.Id == model.CourseId)
           .Select(x => x.Name)
           .FirstOrDefault() ?? "N/A";
    }
    public static string CoursealiasName(ApplicantResponse model)
    {

        //// Get the course name from the model based on the CourseId
        //var courseName = model.Schedule.Campus.Courses
        //    .Where(x => x.Id == model.CourseId)
        //    .Select(x => x.Name)
        //    .FirstOrDefault();

        //if (courseName != null)
        //{
        //    // Check for parentheses in the course name
        //    int startIndex = courseName.IndexOf('(');
        //    int endIndex = courseName.IndexOf(')');

        //    if (startIndex >= 0 && endIndex > startIndex)
        //    {
        //        // Return the name inside the parentheses
        //        return courseName.Substring(startIndex + 1, endIndex - startIndex - 1).Trim();
        //    }

        //    // Return the full course name if no parentheses are found
        //    return courseName;
        //}

        //return string.Empty;

        // Get the course name from the model based on the CourseId
        if (model?.Schedule?.Campus?.Courses == null)
        {
            return string.Empty;
        }

        var courseName = model.Schedule.Campus.Courses
            .Where(x => x.Id == model.CourseId)
            .Select(x => x.Name)
            .FirstOrDefault();

        if (string.IsNullOrEmpty(courseName))
        {
            return string.Empty;
        }

        // Use regex to extract text between parentheses
        var match = Regex.Match(courseName, @"\(([^)]*)\)");
        if (match.Success)
        {
            return match.Groups[1].Value.Trim();
        }

        // Return the full name if no parentheses are found
        return courseName.Trim();
    }

    public static string Interviewer(ApplicantResponse model)
    {
        if (model.Interviews.Count > 0 && model.Interviews.Any(x => x.IsUse))
        {
            var interviewer = model.Interviews.Where(x => x.IsUse)
                                                .Select(x => x.Interviewer)
                                                .FirstOrDefault();
            return interviewer;
        }
        else if (model.Interviews.Count > 0 && model.Interviews.Count(x => x.IsUse) == 0)
        {
            return "No Active Interview";
        }
        else return "Not Recorded";
    }

    public static string ReadingRawScore(ApplicantResponse model)
    {
        return model.Examination != null ? model.Examination.ReadingRawScore.ToString() : "Not Recorded";
    }

    public static string ReadingInterpretation(ApplicantResponse model)
    {
        return model.Examination != null
            ? Assessment.ReadingInterpretation(model.Examination.ReadingRawScore)
            : "Not Recorded";
    }

    public static string ReadingEquivalent(ApplicantResponse model)
    {
        return model.Examination != null
            ? Assessment.ReadingEquivalent(model.Examination.ReadingRawScore).ToString()
            : "Not Recorded";
    }

    public static string MathRawScore(ApplicantResponse model)
    {
        return model.Examination != null ? model.Examination.MathRawScore.ToString() : "Not Recorded";
    }

    public static string MathInterpretation(ApplicantResponse model)
    {
        return model.Examination != null
            ? Assessment.MathInterpretation(model.Examination.MathRawScore)
            : "Not Recorded";
    }

    public static string MathEquivalent(ApplicantResponse model)
    {
        return model.Examination != null
            ? Assessment.MathEquivalent(model.Examination.MathRawScore).ToString()
            : "Not Recorded";
    }

    public static string ScienceRawScore(ApplicantResponse model)
    {
        return model.Examination != null ? model.Examination.ScienceRawScore.ToString() : "Not Recorded";
    }

    public static string ScienceInterpretation(ApplicantResponse model)
    {
        return model.Examination != null
            ? Assessment.ScienceInterpretation(model.Examination.ScienceRawScore)
            : "Not Recorded";
    }

    public static string ScienceEquivalent(ApplicantResponse model)
    {
        return model.Examination != null
            ? Assessment.ScienceEquivalent(model.Examination.ScienceRawScore).ToString()
            : "Not Recorded";
    }

    public static string TotalAchievementEquivalent(ApplicantResponse model)
    {
        return model.Examination != null
            ? Assessment.TotalAchievementEquivalent(model.Examination.ReadingRawScore, model.Examination.MathRawScore, model.Examination.ScienceRawScore).ToString()
            : "Not Recorded";
    }

    public static string IntelligenceRawScore(ApplicantResponse model)
    {
        return model.Examination != null ? model.Examination.IntelligenceRawScore.ToString() : "Not Recorded";
    }

    public static string IntelligenceInterpretation(ApplicantResponse model)
    {
        return model.Examination != null
            ? Assessment.IntelligenceInterpretation(model.Examination.IntelligenceRawScore)
            : "Not Recorded";
    }

    public static string IntelligenceEquivalent(ApplicantResponse model)
    {
        return model.Examination != null
            ? Assessment.IntelligenceEquivalent(model.Examination.IntelligenceRawScore).ToString()
            : "Not Recorded";
    }

    public static string TotalAchievementIntelligenceEquivalent(ApplicantResponse model)
    {
        return model.Examination != null
            ? Assessment.TotalAchievementIntelligenceEquivalent(model.Examination.ReadingRawScore, model.Examination.MathRawScore,
                model.Examination.ScienceRawScore, model.Examination.IntelligenceRawScore).ToString()
            : "Not Recorded";
    }

    public static string ExaminationResult(ApplicantResponse model)
    {
        return model.Examination != null
            ? Assessment.ExaminationResult(model.Examination.ReadingRawScore, model.Examination.MathRawScore,
                model.Examination.ScienceRawScore, model.Examination.IntelligenceRawScore).ToString()
            : "Not Recorded";
    }

    public static string ReadingComprehension(ApplicantResponse model)
    {
        if (model.Interviews.Count > 0 && model.Interviews.Any(x => x.IsUse))
        {
            var score = model.Interviews.Where(x => x.IsUse)
                                                .Select(x => x.InterviewReading)
                                                .FirstOrDefault();
            return score.ToString();
        }
        else if (model.Interviews.Count > 0 && model.Interviews.Count(x => x.IsUse) == 0)
        {
            return "No Active Interview";
        }
        else return "Not Recorded";
    }

    public static string ReadingComprehensionPercentage(ApplicantResponse model)
    {
        if (model.Interviews.Any(x => x.IsUse))
        {
            var firstInterview = model.Interviews.First(x => x.IsUse);
            return Assessment.ReadingComprehensionPercentage(firstInterview.InterviewReading).ToString();
        }
        return "Not Recorded";
    }


    public static string CommunicationVerbal(ApplicantResponse model)
    {
        if (model.Interviews.Count > 0 && model.Interviews.Any(x => x.IsUse))
        {
            var score = model.Interviews.Where(x => x.IsUse)
                                                .Select(x => x.InterviewCommunication)
                                                .FirstOrDefault();
            return score.ToString();
        }
        else if (model.Interviews.Count > 0 && model.Interviews.Count(x => x.IsUse) == 0)
        {
            return "No Active Interview";
        }
        else return "Not Recorded";
    }

    public static string CommunicationVerbalPercentage(ApplicantResponse model)
    {
        if (model.Interviews.Any(x => x.IsUse))
        {
            var firstInterview = model.Interviews.First(x => x.IsUse);
            return Assessment.CommunicationVerbalPercentage(firstInterview.InterviewCommunication).ToString();
        }
        return "Not Recorded";
    }


    public static string AnalyticalAbility(ApplicantResponse model)
    {
        if (model.Interviews.Count > 0 && model.Interviews.Any(x => x.IsUse))
        {
            var score = model.Interviews.Where(x => x.IsUse)
                                                .Select(x => x.InterviewAnalytical)
                                                .FirstOrDefault();
            return score.ToString();
        }

        else if (model.Interviews.Count > 0 && model.Interviews.Count(x => x.IsUse) == 0)
        {
            return "No Active Interview";
        }
        else return "Not Recorded";
    }

    public static string AnalyticalAbilityPercentage(ApplicantResponse model)
    {
        if (model.Interviews.Any(x => x.IsUse))
        {
            var firstInterview = model.Interviews.First(x => x.IsUse);
            return Assessment.AnalyticalAbilityPercentage(firstInterview.InterviewAnalytical).ToString();
        }
        return "Not Recorded";
    }

    public static string ReadingCommunicationAnalyticalPercentage(ApplicantResponse model)
    {
        if (model.Interviews.Any(x => x.IsUse))
        {
            var firstInterview = model.Interviews.First(x => x.IsUse);
            return Assessment.ReadingCommunicationAnalyticalPercentage(firstInterview.InterviewReading, firstInterview.InterviewCommunication, firstInterview.InterviewAnalytical).ToString();
        }
        return "Not Recorded";
    }
    public static string InterviewTotal(ApplicantResponse model)
    {
        if (model.Interviews.Any(x => x.IsUse))
        {
            var firstInterview = model.Interviews.First(x => x.IsUse);
            return Assessment.InterviewTotal(firstInterview.InterviewReading, firstInterview.InterviewCommunication, firstInterview.InterviewAnalytical).ToString();
        }
        return "Not Recorded";
    }
    public static string InterviewResult(ApplicantResponse model)
    {
        if (model.Interviews.Any(x => x.IsUse))
        {
            var firstInterview = model.Interviews.First(x => x.IsUse);
            return Assessment.InterviewResult(firstInterview.InterviewReading, firstInterview.InterviewCommunication, firstInterview.InterviewAnalytical, model.GWA ?? 0).ToString();
        }
        return "Not Recorded";
    }

    public static string OverallTotalRating(ApplicantResponse model)
    {
        if (model.Examination != null && model.Interviews.Any(x => x.IsUse))
        {
            var firstInterview = model.Interviews.First(x => x.IsUse);
            return Assessment.OverallTotalRating(model.Examination.ReadingRawScore, model.Examination.MathRawScore, model.Examination.ScienceRawScore, model.Examination.IntelligenceRawScore,
                firstInterview.InterviewReading, firstInterview.InterviewCommunication, firstInterview.InterviewAnalytical, model.GWA ?? 0).ToString();
        }
        return "Incomplete Data";
    }

    public static string Remarks(ApplicantResponse model)
    {
        if (model.Examination != null && model.Interviews.Any(x => x.IsUse) && model.GWA != null)
        {
            var firstInterview = model.Interviews.First(x => x.IsUse);
            return Assessment.Remarks(model.Examination.ReadingRawScore, model.Examination.MathRawScore, model.Examination.ScienceRawScore, model.Examination.IntelligenceRawScore,
                firstInterview.InterviewReading, firstInterview.InterviewCommunication, firstInterview.InterviewAnalytical, model.GWA ?? 0).ToString();
        }
        return "Incomplete Data";
    }
}
