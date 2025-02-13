using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Screening.Infrastructure.Context;
using System.Data.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using Screening.Infrastructure.Models;
using Screening.Common.Models.Users;
using Screening.Common.Wrapper;
using Screening.Common.Models.Auth;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Net.Http.Headers;
using Screening.Service.ApiServices.CampusServices;
using Screening.Service.ApiServices.DepartmentServices;
using Screening.Service.ApiServices._Connect;
using Screening.Service.ApiServices.AuthServices;
using Screening.Service.ApiServices.UserServices;
using Screening.Service.ApiServices.CourceServices;
using Screening.Service.ApiServices.ScheduleServices;
using Screening.Domain.Entities;
using Screening.Common.Extensions;
using Screening.Common.Models.Schedules;
using Screening.Service.ApiServices.ExaminationServices;
using Screening.Service.ApiServices.InterviewServices;
using Screening.Service.ApiServices.ApplicantServices;
using Screening.Service.ApiServices.RegistrationServices.Undergraduate;
using Screening.Common.Models.Applicants;

namespace Screening.Test;
public class TestBaseIntegration : IDisposable
{
    protected IConnectService Connect;
    protected HttpClient Http { get; }
    WebApplicationFactory<Api.Program> App { get; }
    public TestBaseIntegration()
    {        

        App = new WebApplicationFactory<Api.Program>()
           .WithWebHostBuilder(builder =>
           {
               builder.ConfigureServices(services =>
               {
                   services.RemoveAll<DbContextOptions<ApplicationDbContext>>();
                   services.RemoveAll<DbConnection>();

                   services.AddSingleton<DbConnection>(container =>
                   {
                       var connection = new SqliteConnection("DataSource=:memory:");
                       connection.Open();
                       return connection;
                   });

                   services.AddDbContext<ApplicationDbContext>((container, option) =>
                   {
                       var connection = container.GetRequiredService<DbConnection>();
                       option.UseSqlite(connection);
                   });
               });

               builder.UseEnvironment("Development");
           });

        Http = App.CreateClient();
        var authService = new AuthService(Http);
        var userService = new UserService(Http);
        var campusService = new CampusService(Http);
        var departmentService = new DepartmentService(Http);
        var courseService = new CourseService(Http);
        var scheduleService = new ScheduleService(Http);
        var examinationService = new ExaminationService(Http);
        var interviewService = new InterviewService(Http);
        var applicantService = new ApplicantService(Http);
        var undergraduateService = new UndergraduateRegistrationService(Http);
        Connect = new ConnectService
        (
            authService,
            userService,            
            campusService, 
            departmentService,
            courseService,
            scheduleService,
            examinationService,
            interviewService,
            applicantService,
            undergraduateService
        );

        using (var scope = App.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureCreated();
            SeedRolesAsync(scope.ServiceProvider).GetAwaiter().GetResult();
            SeedUserAsync(scope.ServiceProvider).GetAwaiter().GetResult();
            SeedScheduleForTest(scope.ServiceProvider).GetAwaiter().GetResult();
            SeedApplicantsForTest(scope.ServiceProvider).GetAwaiter().GetResult();
        }
    }

    private async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var roles = new List<string> { "Administrator", "Manager", "Registrar" };

        foreach (var role in roles)
        {
            var roleExist = await roleManager.RoleExistsAsync(role);
            if (!roleExist)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(role));
            }
        }
    }

    private async Task SeedUserAsync(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();

        var user = await userManager.FindByNameAsync("admin1234");
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = "admin1234",
                FirstName = "Turok",
                LastName = "Makto",
                IsActive = true,
                Access = "All"
            };

            var result = await userManager.CreateAsync(user, "!P@ssw0rd");

            if (result.Succeeded)
            {
                var roleExists = await roleManager.RoleExistsAsync("Administrator");
                if (!roleExists)
                {
                    await roleManager.CreateAsync(new IdentityRole<int>("Administrator"));
                }

                await userManager.AddToRoleAsync(user, "Administrator");
            }
            else
            {
                throw new Exception("Failed to create user: " + string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        var user2 = await userManager.FindByNameAsync("InActiveUser");
        if (user2 == null)
        {
            user2 = new ApplicationUser
            {
                UserName = "InActiveUser",
                FirstName = "TestFirst",
                LastName = "TestLast",
                IsActive = false,
                Access = null
            };

            var result = await userManager.CreateAsync(user2, "!P@ssw0rd");            
        }
    }

    private async Task SeedScheduleForTest(IServiceProvider serviceProvider)
    {
        var Context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        var user = Context.Users.FirstOrDefault();
        var createdBy = user != null
        ? $"{user.FirstName} {user.LastName}"
        : "Default";

        var campus = new Campus
        {
            Name = "Test Campus",
            Address = "Test Address",
            HasDepartment = true,
            DateCreated = DateTime.Now,
            CreatedBy = createdBy
        };
        Context.Campuses.Add(campus);
        await Context.SaveChangesAsync();

        var department = new Department
        {
            Name = "Test Department",
            CampusId = campus.Id,
            Campus = campus,
            DateCreated = DateTime.Now,
            CreatedBy = createdBy
        };
        Context.Departments.Add(department);
        await Context.SaveChangesAsync();

        var course = new Course
        {
            Name = "Test Course",
            CampusId = campus.Id,
            Campus = campus,
            DepartmentId = department.Id,
            Department = department,
            DateCreated = DateTime.Now,
            CreatedBy = createdBy
        };
        Context.Courses.Add(course);

        var course2 = new Course
        {
            Name = "Test Course2",
            CampusId = campus.Id,
            Campus = campus,
            DepartmentId = department.Id,
            Department = department,
            DateCreated = DateTime.Now,
            CreatedBy = createdBy
        };
        Context.Courses.Add(course2);


        var schedule = new Schedule
        {
            Campus = campus,
            CampusId = campus.Id,
            ScheduleDate = DateTime.Now.AddDays(10),
            Time = "8:00am",
            Slot = 400,
            Venue = "Test Venue",
            SchoolYear = $"{DateTime.Now.AddDays(10).Year.ToString()} - {(DateTime.Now.AddDays(10).Year + 1).ToString()}",
            DateCreated = DateTime.Now,
            CreatedBy = createdBy,
        };
        Context.Schedules.Add(schedule);

        var schedule2 = new Schedule
        {
            Campus = campus,
            CampusId = campus.Id,
            ScheduleDate = DateTime.Now.AddDays(10),
            Time = "1:00am",
            Slot = 400,
            Venue = "Test Venue",
            SchoolYear = $"{DateTime.Now.AddDays(10).Year.ToString()} - {(DateTime.Now.AddDays(10).Year + 1).ToString()}",
            DateCreated = DateTime.Now,
            CreatedBy = createdBy,
        };
        Context.Schedules.Add(schedule2);
        await Context.SaveChangesAsync();
    }

    private async Task SeedApplicantsForTest(IServiceProvider serviceProvider)
    {
        var Context = serviceProvider.GetRequiredService<ApplicationDbContext>();

        var schedule = Context.Schedules.FirstOrDefault();

        var applicant = new Applicant
        {
            ScheduleId = schedule.Id,
            CourseId = schedule.Campus.Schedules.Select(x => x.Id).FirstOrDefault(),
            TransactionDate = DateTime.Now,
            ApplicantStatus = "New",
            Track = "GAS",
            ControlNo = "123",

        };
        Context.Applicants.Add(applicant);
        await Context.SaveChangesAsync();

        var firstAppRequest = new FirstApplicationInfo
        {
            ApplicantId = applicant.Id,
            CourseId = applicant.CourseId,
            ScheduleId = applicant.ScheduleId,
            ApplicantStatus = applicant.ApplicantStatus,
            TransactionDate = DateTime.Now,
            Track = applicant.Track
        };
        Context.FirstApplicationInfos.Add(firstAppRequest);
        await Context.SaveChangesAsync();

        var perInfRequest = new PersonalInformation
        {
            ApplicantId = applicant.Id,
            FirstName = "Albert",
            MiddleName = "Tupaz",
            LastName = "Lamadrid",
            NickName = "Bert",
            Sex = "Male",
            CivilStatus = "Married",
            PlaceOfBirth = "Catbalogan City",
            Citizenship = "Filipino",
            Religion = "Catholic",
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
            CurrentHouseNo = "163",
            CurrentStreet = "San Francisco St",
            CurrentBarangay = "08",
            CurrentPurok = "1",
            CurrentMunicipality = "Catbalogan City",
            CurrentProvince = "Samar",
            CurrentZipCode = "6700",
            Dialect = "Waray",
            IsIndigenous = false,
            TribalAffiliation = "N/A",
            HouseHold4psNumber = "N/A",
            BirthOrder = 1,
            Brothers = 3,
            Sisters = 0,
            Is4psMember = false,
            NameExtension = null
        };
        Context.PersonalInformations.Add(perInfRequest);
        await Context.SaveChangesAsync();

        var spouseRequest = new Spouse
        {
            ApplicantId = applicant.Id,
            FullName = "Analyn U. Lamadrid",
            ContactNumber = "09998789632",
            Education = "High School Graduate",
            Occupation = "None",
            OfficeAddress = "N/A",
            Barangay = "08",
            Municipality = "Catbalogan City",
            Province = "Samar",
            ZipCode = "6700",
            Birthday = new DateTime(1990, 5, 5),
            BirthPlace = "Catbalogan City"
        };
        Context.Spouses.Add(spouseRequest);
        await Context.SaveChangesAsync();

        var acadRequest = new AcademicBackground
        {
            ApplicantId = applicant.Id,
            SchoolAttended = "Samar National School",
            SchoolAddress = "Catbalogan City",
            LRN = "012345678912",
            Awards = "N/A",
            YearGraduated = 2023,
            SchoolSector = "Government",
            ElementarySchoolAttended = "Catbalogan II Central School",
            ElementaryInclusiveYear = "2010 - 2016",
            ElementarySchoolAddress = "Catbalogan City",
            ElementaryAwards = "N/A",
            JuniorSchoolAttended = "Samar National School",
            JuniorInclusiveYear = "2017-2022",
            JuniorSchoolAddress = "Catbalogan City",
            JuniorAward = "N/A",
            SeniorSchoolAttended = "Samar National School",
            SeniorInclusiveYear = "2021-2023",
            SeniorSchoolAddress = "Catbalogan City",
            SeniorAwards = "N/A",
            CollegeSchoolAttended = "N/A",
            CollegeSchoolAddress = "N/A",
            CollegeAwards = "N/A",
            CollegeInclusiveYear = "N/A",
            GraduateSchoolAttended = "N/A",
            GraduateInclusiveYear = "N/A",
            GraduateSchoolAddress = "N/A",
            GraduateAwards = "N/A",
            PostGraduateSchoolAttended = "N/A",
            PostGraduaterInclusiveYear = "N/A",
            PostGraduateSchoolAddress = "N/A",
            PostGraduateAwards = "N/A",
            BenefactorRelation = "N/A",
            Organization = "N/A",
            EducationBenefactor = "N/A",
            Scholarship = "N/A",
            Motto = "N/A",
            PlanAfterCollege = "Work",
            Skills = "Programming",
        };
        Context.AcademicBackgrounds.Add(acadRequest);
        await Context.SaveChangesAsync();

        var parRequest = new ParentGuardianInformation
        {
            ApplicantId = applicant.Id,
            FatherFirstName = "Johny",
            FatherMiddleName = "Melborne",
            FatherLastName = "Adams",
            FatherContactNo = "123-456-7890",
            FatherCitizenship = "American",
            FatherEmail = "john.doe@example.com",
            FatherOccupation = "Engineer",
            MotherFirstName = "Jane",
            MotherMiddleName = "Quincy",
            MotherLastName = "Adams",
            MotherContactNo = "098-765-4321",
            MotherCitizenship = "American",
            MotherEmail = "jane.doe@example.com",
            MotherOccupation = "Teacher",
            GuardianFirstName = "Jack",
            GuardianMiddleName = "B",
            GuardianLastName = "Smith",
            GuardianContactNo = "555-123-4567",
            GuardianCitizenship = "American",
            GuardianEmail = "jack.smith@example.com",
            GuardianOccupation = "Accountant",
            FatherBirthday = new DateTime(1970, 1, 15),
            FatherBirthPlace = "New York",
            FatherReligion = "Christian",
            FatherMaritalStatus = "Married",
            FatherDialect = "English",
            FatherPermanentAddress = "123 Elm Street, New York, NY 10001",
            FatherEducation = "Bachelor's Degree",
            FatherEstimatedMonthly = "5000",
            FatherOtherIncome = "200",
            MotherBirthday = new DateTime(1972, 5, 22),
            MotherBirthPlace = "Los Angeles",
            MotherReligion = "Christian",
            MotherMaritalStatus = "Married",
            MotherDialect = "English",
            MotherPermanentAddress = "123 Elm Street, New York, NY 10001",
            MotherEducation = "Master's Degree",
            MotherEstimatedMonthly = "4500",
            MotherOtherIncome = "150",
            GuardianBirthday = new DateTime(1965, 9, 30),
            GuardianBirthPlace = "Chicago",
            GuardianReligion = "Christian",
            GuardianMaritalStatus = "Single",
            GuardianDialect = "English",
            GuardianPermanentAddress = "456 Oak Avenue, Chicago, IL 60614",
            GuardianEducation = "Associate Degree",
            GuardianEstimatedMonthly = "3000",
            GuardianOtherIncome = "100"
        };
        Context.ParentGuardianInformations.Add(parRequest);

        for (int i = 1; i <= (applicant.PersonalInformation.Brothers + applicant.PersonalInformation.Sisters); i++)
        {
            var famRequest = new FamilyRelation
            {
                ApplicantId = applicant.Id,
                FullName = "Brother" + i,
                GradeCourse = "Grade" + i,
                MonthlyIncome = "N/A",
                SchoolOccupation = "N/A",
                Sex = "Male",
                Birthday = new DateTime(2000 + i, 5 + i, 25 + i),
            };
            Context.FamilyRelations.Add(famRequest);
            await Context.SaveChangesAsync();
        }

        var physicalRequest = new PhysicalHealth
        {
            ApplicantId = applicant.Id,
            AccidentsExperienced = "None",
            ChronicIllnesses = "None",
            IsWithDisability = false,
            Medicines = "None",
            OperationsExperienced = "None",
        };
        Context.PhysicalHealths.Add(physicalRequest);
        await Context.SaveChangesAsync();

        Random random = new Random();
        bool response = random.Next(2) == 0;
        if (response == true)
        {
            var councelorRequest = new CouncelorConsultation
            {
                ApplicantId = applicant.Id,
                Start = new DateTime(2023, 5, 5),
                End = new DateTime(2023, 11, 6),
                Sessions = 2,
                Reasons = "Too Personal"
            };
            Context.CouncelorConsultations.Add(councelorRequest);
            await Context.SaveChangesAsync();
        }

        response = random.Next(2) == 0;
        if (response == true)
        {
            var psychiatristRequest = new PsychiatristConsultation
            {
                ApplicantId = applicant.Id,
                Start = new DateTime(2023, 5, 5),
                End = new DateTime(2023, 11, 6),
                Sessions = 2,
                Reasons = "Too Personal"
            };
            Context.PsychiatristConsultations.Add(psychiatristRequest);
            await Context.SaveChangesAsync();
        }

        response = random.Next(2) == 0;
        if (response == true)
        {
            var psychologistRequest = new PsychologistConsultation
            {
                ApplicantId = applicant.Id,
                Start = new DateTime(2023, 5, 5),
                End = new DateTime(2023, 11, 6),
                Sessions = 2,
                Reasons = "Too Personal"
            };
            Context.PsychologistConsultations.Add(psychologistRequest);
            await Context.SaveChangesAsync();
        }

        var personalityRequest = new PersonalityProfile
        {
            ApplicantId = applicant.Id,
            Active = true,
            Adaptable = true,
            Cautious = true,
            Confident = true,
            Conforming = true,
            Creative = true,
            Conscientious = true,
            Friendly = true,
            Generous = true,
            GoodNatured = true,
            EmotionallyStable = true,
            HabituallySilent = true,
            Industrious = true,
            Organized = true,
            PreferredByGroups = true,
            Polite = true,
            Resourceful = true,
            Outgoing = false,
            TakesChargeWhenAssigned = true,
            Truthful = true,
            VolunteersToLead = false,
            WellGroomed = true,
            WorksWillWithOthers = true,
            WorksPromptly = true,
            SelfControl = true,
            Studies = false,
            Family = false,
            Friend = false,
            Problems = "",
            ComfortableDiscussing = "Sample",
            Self = false,
            Specify = "Sample"
        };
        Context.PersonalityProfiles.Add(personalityRequest);
        await Context.SaveChangesAsync();

        var emergencyRequest = new EmergencyContact
        {
            ApplicantId = applicant.Id,
            Address = "Catbalogan City",
            ContactNo = "09998887456",
            Name = "Mikee Doe",
            Relationship = "Relatives"
        };
        Context.EmergencyContacts.Add(emergencyRequest);
        await Context.SaveChangesAsync();

        var registeredRequest = new Registered
        {
            ApplicantId = applicant.Id,
            RegistrationDate = DateTime.Now,
        };
        Context.Registereds.Add(registeredRequest);
        await Context.SaveChangesAsync();
    }
    

    protected async Task<ResponseWrapper<UserResponse>> LoginDefault()
    {
        var model = new AuthRequest { Username = "admin1234", Password = "!P@ssw0rd" };
        var loginResult = await Connect.Auth.Login(model);

        if (loginResult.IsSuccessful && !string.IsNullOrEmpty(loginResult.Data?.AccessToken))
        {
            Http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", loginResult.Data.AccessToken);
        }

        return loginResult;
    }

    protected async Task<ResponseWrapper<PagedList<ScheduleResponse>>> SchedulesDefault()
    {
        var user = await LoginDefault();
        var listQuery = new DataGridQuery
        {
            Page = 0,
            PageSize = 100,
            SortField = nameof(Schedule.ScheduleDate),
            SortDir = DataGridQuerySortDirection.Ascending
        };

        var scheduleModels = await Connect.Schedule.List(listQuery, user.Data.Access);
        return scheduleModels;
    }

    protected async Task<ResponseWrapper<PagedList<ApplicantResponse>>> ApplicantsDefault()
    {
        var user = await LoginDefault();
        var listQuery = new DataGridQuery
        {
            Page = 0,
            PageSize = 100,
            SortField = nameof(Applicant.PersonalInformation.LastName),
            SortDir = DataGridQuerySortDirection.Ascending
        };

        var applicantModels = await Connect.Applicant.List(listQuery, null, user.Data.Access);
        return applicantModels;
    }

    public void Dispose()
    {
        using var scope = App.Services.CreateScope();
        var dataContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dataContext.Database.EnsureDeleted();
        dataContext.Database.CloseConnection();

        dataContext.Dispose();
    }
}
