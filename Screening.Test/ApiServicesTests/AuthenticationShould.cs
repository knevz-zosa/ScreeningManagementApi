using Microsoft.AspNetCore.Identity.Data;
using Screening.Common.Models.Auth;
using Screening.Common.Models.Users;
using System;

namespace Screening.Test.ApiServicesTests;
public class AuthenticationShould : TestBaseIntegration
{
    [Fact]
    public async Task PerformAuthenticationMethods()
    {
        //Validation: Try logging in with invalid credentials, expect failure
        var request = new AuthRequest
        {

            Username = "admin12345",
            Password = "!P@ssw0rd123"
        };

        // Act: Attempt to log in
        var invalidCredentialResponse = await Connect.Auth.Login(request);

        // Assert: Verify that the operation failed and check the error message
        Assert.False(invalidCredentialResponse.IsSuccessful);
        Assert.Contains("Invalid credentials.", invalidCredentialResponse.Messages.FirstOrDefault());


        //Validation: Try logging in with an inactive account, expect failure
        request = new AuthRequest
        {
            Username = "InActiveUser",
            Password = "!P@ssw0rd"
        };

        // Act: Attempt to log in
        var inActiveResponse = await Connect.Auth.Login(request);

        // Assert: Verify that the operation failed and check the error message
        Assert.False(inActiveResponse.IsSuccessful);
        Assert.Contains("Your account is inactive, please contact Administrator.", inActiveResponse.Messages.FirstOrDefault());


        // Arrange: Valid credentials for login
        request = new AuthRequest
        {
            Username = "admin1234",
            Password = "!P@ssw0rd"
        };

        // Act: Attempt to log in
        var validLoginResponse = await Connect.Auth.Login(request);

        // Assert: Verify login was successful
        Assert.NotNull(validLoginResponse);
        Assert.NotNull(validLoginResponse.Data);
        Assert.NotNull(validLoginResponse.Data.AccessToken);
        Assert.NotNull(validLoginResponse.Data.RefreshToken);
        Assert.True(validLoginResponse.IsSuccessful);

      
    }
}
