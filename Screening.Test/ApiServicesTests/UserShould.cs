using Microsoft.AspNetCore.Identity;
using Screening.Common.Extensions;
using Screening.Common.Models.Users;
using Screening.Infrastructure.Models;

namespace Screening.Test.ApiServicesTests;
public class UserShould : TestBaseIntegration
{     
    [Fact]
    public async Task PerformUsersMethods()
    {           

        #region REGISTRATION
        //Validation: Try creating with username contain white space, expect failure
        var createRequest = new UserRequest
        {
            FirstName = "User",
            LastName = "Admin",
            UserName = "admin admin",
            Password = "!P@ssw0rd",
            ConfirmPassword = "!P@ssw0rd"
        };

        // Act: Attempt to create the user
        var invalidUserNameResponse = await Connect.User.Create(createRequest);

        // Assert: Verify that the operation failed and check the error message
        Assert.False(invalidUserNameResponse.IsSuccessful);
        Assert.Contains($"Registration failed: Username '{createRequest.UserName}' is invalid, can only contain letters or digits.", invalidUserNameResponse.Messages.FirstOrDefault());

        //Validation: Try creating with username already taken, expect failure
        createRequest = new UserRequest
        {

            FirstName = "User",
            LastName = "Admin",
            UserName = "admin1234",
            Password = "!P@ssw0rd",
            ConfirmPassword = "!P@ssw0rd"
        };

        // Act: Attempt to create the user
        var takenUsernameResponse = await Connect.User.Create(createRequest);

        // Assert: Verify that the operation failed and check the error message
        Assert.False(takenUsernameResponse.IsSuccessful);
        Assert.Contains($"Registration failed: Username '{createRequest.UserName}' is already taken.", takenUsernameResponse.Messages.FirstOrDefault());


        //Validation: Try creating with first name and last name already exist, expect failure
        createRequest = new UserRequest
        {
            FirstName = "Turok",
            LastName = "Makto",
            UserName = "admin123456",
            Password = "!P@ssw0rd",
            ConfirmPassword = "!P@ssw0rd"
        };

        // Act: Attempt to create the user
        var takenProfileResponse = await Connect.User.Create(createRequest);

        // Assert: Verify that the operation failed and check the error message
        Assert.False(takenProfileResponse.IsSuccessful);
        Assert.Contains("Registration failed: A user with the same full name already exists.", takenProfileResponse.Messages.FirstOrDefault());


        //Validation: Try creating with password contain white space, expect failure
        createRequest = new UserRequest
        {
            FirstName = "User",
            LastName = "Admin",
            UserName = "adminadmin",
            Password = "!P@ss w0rd",
            ConfirmPassword = "!P@ss w0rd"
        };

        // Act: Attempt to create the user
        var invalidPasswordResponse = await Connect.User.Create(createRequest);

        // Assert: Verify that the operation failed and check the error message
        Assert.False(invalidPasswordResponse.IsSuccessful);
        Assert.Contains("Registration failed: Password cannot contain spaces.", invalidPasswordResponse.Messages.FirstOrDefault());

        
        // Arrange: Create user
        createRequest = new UserRequest
        {
            FirstName = "User",
            LastName = "Admin",
            UserName = "adminadmin",
            Password = "!P@ssw0rd",
            ConfirmPassword = "!P@ssw0rd"
        };

        var createResult = await Connect.User.Create(createRequest);
        Assert.True(createResult.IsSuccessful);
        Assert.Contains("Registration successful.", createResult.Messages);
        var userId = createResult.Data;


        var loginResponse = await LoginDefault();

        // Act & Assert: Verify user was created
        var model = await Connect.User.Get(userId);
        Assert.NotNull(model);
        Assert.Equal("User", model.Data.FirstName);
        Assert.Equal("Admin", model.Data.LastName);
        #endregion


        #region GET
        // Act & Assert: Verify user was existed, expect failure
        var getModel = await Connect.User.Get(10);

        // Assert: Verify that the operation failed and check the error message
        Assert.Null(getModel.Data);
        Assert.False(getModel.IsSuccessful);
        #endregion


        #region LIST
        //LIST USERS
        var listUser = new DataGridQuery
        {
            Page = 0,
            PageSize = 10,
            SortField = nameof(ApplicationUser.LastName),
            SortDir = DataGridQuerySortDirection.Ascending
        };

        var models = await Connect.User.List(listUser);
        Assert.NotNull(models);
        Assert.True(models.IsSuccessful);
        Assert.True(models.Data.List.Any());

        //LIST ROLES
        var listRoles = new DataGridQuery
        {
            Page = 0,
            PageSize = 10,
            SortField = nameof(IdentityRole<int>.Name),
            SortDir = DataGridQuerySortDirection.Ascending
        };

        var roles = await Connect.User.Roles(listRoles);
        Assert.NotNull(roles);
        Assert.True(roles.IsSuccessful);
        Assert.True(roles.Data.List.Any());
        #endregion


        #region UPDATE
        //UPDATE PROFILE FAILURE
        //Validation: Try updating with existing profile, expect failure
        var updateExistingProfileRequest = new UserProfileUpdate
        {
            Id = userId,
            FirstName = "Turok",
            LastName = "Makto"
        };

        // Act: Attempt to update the user's profile
        var updateExistingProfileResponse = await Connect.User.UpdateProfile(updateExistingProfileRequest);

        // Assert: Verify that the operation failed and check the error message
        Assert.False(updateExistingProfileResponse.IsSuccessful);
        Assert.Contains("A user with the same full name already exists.", updateExistingProfileResponse.Messages.FirstOrDefault());

        //UPDATE PROFILE SUCCESSFUL
        // Arrange: Update user profile
        var updateProfileRequest = new UserProfileUpdate
        {
            Id = userId,
            FirstName = "John",
            LastName = "Adams"
        };

        var updateProfileResponse = await Connect.User.UpdateProfile(updateProfileRequest);
        Assert.True(updateProfileResponse.IsSuccessful);
        Assert.Contains("Profile updated successfully.", updateProfileResponse.Messages.FirstOrDefault());

        // Act & Assert: Verify user profile was updated
        var updatedProfileResult = await Connect.User.Get(userId);
        Assert.NotNull(updatedProfileResult);
        Assert.Equal("John", updatedProfileResult.Data.FirstName);
        Assert.Equal("Adams", updatedProfileResult.Data.LastName);


        //UPDATE ROLE FAILURE
        var updateRole = new UserRoleUpdate
        {
            Id = userId,
            Role = "MANAGERS"
        };

        var updateRoleResponse = await Connect.User.UpdateRole(updateRole);
        Assert.False(updateRoleResponse.IsSuccessful);
        Assert.Contains($"Role {updateRole.Role} does not exist.", updateRoleResponse.Messages.FirstOrDefault());

        //UPDATE ROLE SUCCESSFUL
        updateRole = new UserRoleUpdate
        {
            Id = userId,
            Role = "Manager"
        };

        updateRoleResponse = await Connect.User.UpdateRole(updateRole);
        Assert.True(updateRoleResponse.IsSuccessful);
        Assert.Contains("Role updated successfully.", updateRoleResponse.Messages.FirstOrDefault());

        // Act & Assert: Verify user role was updated
        var updatedRoleResult = await Connect.User.Get(userId);
        Assert.NotNull(updatedRoleResult);
        Assert.Equal("Manager", updatedRoleResult.Data.Role);

        //UPDATE ACCESS
        var updateAccess = new UserAccessUpdate
        {
            Id = userId,
            Access = "Main Campus"
        };

        var updateAccessResponse = await Connect.User.UpdateAccess(updateAccess);
        Assert.True(updateAccessResponse.IsSuccessful);
        Assert.Contains("Access updated successfully.", updateAccessResponse.Messages.FirstOrDefault());

        // Act & Assert: Verify user access was updated
        var updatedAccessResult = await Connect.User.Get(userId);
        Assert.NotNull(updatedAccessResult);
        Assert.Equal("Main Campus", updatedAccessResult.Data.Access);

        //UPDATE STATUS
        var updateStatus = new UserStatusUpdate
        {
            Id = userId,
            IsActive = true
        };

        var updateStatusResponse = await Connect.User.UpdateStatus(updateStatus);
        Assert.True(updateStatusResponse.IsSuccessful);
        Assert.Contains("Status updated successfully.", updateStatusResponse.Messages.FirstOrDefault());

        // Act & Assert: Verify user status was updated
        var updatedStatusResult = await Connect.User.Get(userId);
        Assert.NotNull(updatedStatusResult);
        Assert.True(updatedStatusResult.Data.IsActive);
        #endregion


        #region DELETE
        // Arrange: Delete user
        var deleteResult = await Connect.User.Delete(userId);
        Assert.True(deleteResult.IsSuccessful);
        Assert.Contains("User deleted successfully.", deleteResult.Messages.FirstOrDefault());

        // Act & Assert: Verify user was deleted
        models = await Connect.User.List(listUser);
        Assert.NotNull(models);
        Assert.DoesNotContain(models.Data.List, x => x.Id == userId);
        #endregion
    }
}