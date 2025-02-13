using Screening.Common.Extensions;
using Screening.Common.Models.Campuses;
using Screening.Common.Models.Schedules;

namespace Screening.Test.ApiServicesTests;
public class ScheduleShould : TestBaseIntegration
{
    [Fact]
    public async Task PerformSchedulesMethods()
    {
        // Arrange: Authenticate first
        var userData = await LoginDefault();

        // Arrange: Create Campus
        var campusRequest = new CampusRequest
        {
            Name = "Campus123",
            HasDepartment = false,
            Address = "Current Address",
            CreatedBy = $"{userData.Data.FirstName} {userData.Data.LastName}"

        };

        var campusResult = await Connect.Campus.Create(campusRequest);
        Assert.True(campusResult.IsSuccessful);
        var campusId = campusResult.Data;

        // Act & Assert: Verify campus was created
        var campusModel = await Connect.Campus.Get(campusId);
        Assert.NotNull(campusModel.Data);
        Assert.Equal("Campus123", campusModel.Data.Name);

        // Arrange: Create Schedule
        var scheduleRequest = new ScheduleRequest
        {
            ScheduleDate = DateTime.Now.AddDays(1),
            DateCreated = DateTime.Now,
            CampusId = campusId,
            Slot = 40,
            Time = "8:00am",
            Venue = "Gym",
            CreatedBy = $"{userData.Data.FirstName} {userData.Data.LastName}"
        };

        var scheduleResult = await Connect.Schedule.Create(scheduleRequest);
        Assert.True(scheduleResult.IsSuccessful);
        var scheduleId = scheduleResult.Data;

        // Act & Assert: Verify schedule was created
        var scheduleModel = await Connect.Schedule.Get(scheduleId);
        Assert.NotNull(scheduleModel);
        Assert.Equal(DateTime.Now.AddDays(1).Date, scheduleModel.Data.ScheduleDate.Date);
        Assert.Equal("8:00am", scheduleModel.Data.Time);

        // Validation: Try creating another department with the same schedule day and time in the same campus, expect failure
        var duplicateRequest = new ScheduleRequest
        {
            ScheduleDate = DateTime.Now.AddDays(1),
            DateCreated = DateTime.Now,
            CampusId = campusId,
            Slot = 40,
            Time = "8:00am",
            Venue = "Gym",
            CreatedBy = $"{userData.Data.FirstName} {userData.Data.LastName}"
        };

        // Act & Assert: Expect a ResponseException to be thrown for the duplicate
        var exceptionCreate = await Connect.Schedule.Create(duplicateRequest);

        Assert.False(exceptionCreate.IsSuccessful);
        Assert.Contains("Schedule with the same date and time in this campus already exists.", exceptionCreate.Messages.FirstOrDefault());

        // Arrange: Create Different Schedule
        var newScheduleRequest = new ScheduleRequest
        {
            ScheduleDate = DateTime.Now.AddDays(1),
            DateCreated = DateTime.Now,
            CampusId = campusId,
            Slot = 40,
            Time = "1:00pm",
            Venue = "Gym",
            CreatedBy = $"{userData.Data.FirstName} {userData.Data.LastName}"
        };

        var newScheduleResult = await Connect.Schedule.Create(newScheduleRequest);
        Assert.True(newScheduleResult.IsSuccessful);
        var newScheduleId = newScheduleResult.Data;

        // Act & Assert: Verify schedule was created
        var newScheduleModel = await Connect.Schedule.Get(newScheduleId);
        Assert.NotNull(newScheduleModel);
        Assert.Equal(DateTime.Now.AddDays(1).Date, newScheduleModel.Data.ScheduleDate.Date);
        Assert.Equal("1:00pm", newScheduleModel.Data.Time);


        // GET List
        var listQuery = new DataGridQuery
        {
            Page = 0,
            PageSize = 10,
            SortField = nameof(Schedule.ScheduleDate),
            SortDir = DataGridQuerySortDirection.Ascending
        };

        var scheduleModels = await Connect.Schedule.List(listQuery, userData.Data.Access);
        Assert.NotNull(scheduleModels);
        Assert.True(scheduleModels.IsSuccessful);
        Assert.True(scheduleModels.Data.List.Any());


        // Act & Assert: Get SchoolYears
        var schoolYears = await Connect.Schedule.SchoolYears();
        Assert.NotNull(schoolYears.Data);


        // Arrange: Delete schedule
        var deleteModel = await Connect.Schedule.Delete(scheduleId);
        Assert.True(deleteModel.IsSuccessful);

        // Act & Assert: Verify schedule was deleted
        scheduleModels = await Connect.Schedule.List(listQuery, userData.Data.Access);
        Assert.NotNull(scheduleModels);
        Assert.DoesNotContain(scheduleModels.Data.List, x => x.Id == scheduleModel.Data.Id);
    }
}

