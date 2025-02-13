using Screening.Common.Models.Academics;
using Screening.Common.Models.Applicants;
using Screening.Common.Models.EmergencyContacts;
using Screening.Common.Models.PersonalInformations;
using Screening.Common.Models.Transfers;

namespace Screening.Test.ApiServicesTests;
public class ManageApplicantShould : TestBaseIntegration
{
    [Fact]
    public async Task PerformTransfers()
    {
        var user = await LoginDefault();
        var applicants = await ApplicantsDefault();
        var applicant = applicants.Data.List.FirstOrDefault();
        var schedules = await SchedulesDefault();
        var courses = schedules.Data.List.Select(x => x.Campus.Courses).ToArray();

        //Get applicant
        var applicantModel = await Connect.Applicant.Get(applicant.Id);
        Assert.NotNull(applicantModel);
        Assert.NotNull(applicantModel.Data.Schedule);

        //Get applicant old schedule id
        var oldScheduleId = applicantModel.Data.Schedule.Id;
        Assert.Equal(oldScheduleId, applicantModel.Data.ScheduleId);

        // Arrange: transfer applicant to another schedule
        var applicantNewSchedule = new ApplicantTransfer()
        {
            Id = applicantModel.Data.Id,
            ScheduleId = schedules.Data.List.ToArray()[1].Id,
            CourseId = applicantModel.Data.CourseId
        };
        var applicantNewScheduleModel = await Connect.Applicant.UpdateTransfer(applicantNewSchedule);
        Assert.True(applicantNewScheduleModel.IsSuccessful);

        // Assert: Verify applicant's schedule have changed
        applicantModel = await Connect.Applicant.Get(applicant.Id);
        Assert.NotNull(applicantModel);
        Assert.NotNull(applicantModel.Data.Schedule);
        Assert.NotEqual(oldScheduleId, applicantModel.Data.ScheduleId);
    }

    [Fact]
    public async Task PerformUpdateEmergencyContact()
    {
        var user = await LoginDefault();
        var applicants = await ApplicantsDefault();
        var applicant = applicants.Data.List.FirstOrDefault();

        //Get applicant
        var applicantModel = await Connect.Applicant.Get(applicant.Id);
        Assert.NotNull(applicantModel);
        Assert.NotNull(applicantModel.Data.Schedule);

        var updateEmergencyContact = new EmergencyContactUpdate()
        {
            Id = applicantModel.Data.EmergencyContact.Id,
            ApplicantId = applicantModel.Data.Id,
            Name = "New Contact Person",
            ContactNo = "09997778885",
            Address = "New Address",
            Relationship = "New Relationship"
        };

        var result = await Connect.Applicant.UpdateEmergencyContact(updateEmergencyContact);
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public async Task PerformUpdateGWAStatusTrack()
    {
        var user = await LoginDefault();
        var applicants = await ApplicantsDefault();
        var applicant = applicants.Data.List.FirstOrDefault();

        //Get applicant
        var applicantModel = await Connect.Applicant.Get(applicant.Id);
        Assert.NotNull(applicantModel);
        Assert.NotNull(applicantModel.Data.Schedule);

        var updateGwaStatusTrack = new ApplicantUpdateGwaStatusTrack()
        {
            Id = applicantModel.Data.Id,
            GWA = 87.80,
            ApplicantStatus = "New",
            Track = "HUMS"            
        };

        var result = await Connect.Applicant.UpdateGWAStatusTrack(updateGwaStatusTrack);
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public async Task PerformUpdateLRN()
    {
        var user = await LoginDefault();
        var applicants = await ApplicantsDefault();
        var applicant = applicants.Data.List.FirstOrDefault();

        //Get applicant
        var applicantModel = await Connect.Applicant.Get(applicant.Id);
        Assert.NotNull(applicantModel);
        Assert.NotNull(applicantModel.Data.Schedule);
        
        var updateLrn = new LrnUpdate()
        {
            Id = applicantModel.Data.AcademicBackground.Id,
            ApplicantId= applicantModel.Data.Id,
            LRN = "112345678911"
        };

        var result = await Connect.Applicant.UpdateLRN(updateLrn);
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public async Task PerformUpdatePersonalInformation()
    {
        var user = await LoginDefault();
        var applicants = await ApplicantsDefault();
        var applicant = applicants.Data.List.FirstOrDefault();

        //Get applicant
        var applicantModel = await Connect.Applicant.Get(applicant.Id);
        Assert.NotNull(applicantModel);
        Assert.NotNull(applicantModel.Data.Schedule);

        var updatePersonalInformation = new PersonalInformationUpdate()
        {
            Id = applicantModel.Data.PersonalInformation.Id,
            ApplicantId = applicantModel.Data.Id,
            FirstName = "Johnny",
            MiddleName = "Quincy",
            LastName = "Adams",
            NickName = "John",
            Sex = "Male",
            PlaceOfBirth = "Catbalogan City",
            Citizenship = "Filipino",
            Email = "test@email.com",
            ContactNumber = "09998798521",
            DateofBirth = new DateTime(1990, 1, 1),
            HouseNo = "163",
            Street = "San Francisco St",
            Barangay = "08",
            Purok = "1",
            Municipality = "Catbalogan City",
            Province = "Samar",
            ZipCode = "6700",
            NameExtension = "Jr."
        };

        var result = await Connect.Applicant.UpdatePersonalInformation(updatePersonalInformation);
        Assert.True(result.IsSuccessful);
    }

    [Fact]
    public async Task PerformUpdateStudentId()
    {
        var user = await LoginDefault();
        var applicants = await ApplicantsDefault();
        var applicant = applicants.Data.List.FirstOrDefault();

        //Get applicant
        var applicantModel = await Connect.Applicant.Get(applicant.Id);
        Assert.NotNull(applicantModel);
        Assert.NotNull(applicantModel.Data.Schedule);

        var updateStudentId = new ApplicantUpdateStudentId()
        {
            Id = applicantModel.Data.Id,
            StudentId = "115-63-5"
        };

        var result = await Connect.Applicant.UpdateStudentId(updateStudentId);
        Assert.True(result.IsSuccessful);
    }
}
