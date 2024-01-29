using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bogus;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NightTasker.Common.Core.Identity.Contracts;
using NightTasker.UserHub.Core.Application.Features.Organizations.Models;
using NightTasker.UserHub.Core.Application.Models.Organization;
using NightTasker.UserHub.Core.Domain.Entities;
using NightTasker.UserHub.Core.Domain.Enums;
using NightTasker.UserHub.Infrastructure.Persistence;
using NightTasker.UserHub.IntegrationTests.Framework;
using NightTasker.UserHub.Presentation.WebApi.Constants;
using NightTasker.UserHub.Presentation.WebApi.Endpoints;
using NightTasker.UserHub.Presentation.WebApi.Requests.Organization;
using NightTasker.UserHub.Presentation.WebApi.Responses.Organization;
using NSubstitute;
using Xunit;

namespace NightTasker.UserHub.Presentation.WebApi.IntegrationTests.Controllers.V1;

public class OrganizationControllerTests() : BaseIntegrationTests(CreateMockedServices(), true)
{
    private readonly Faker _faker = new();
    private static readonly Guid UserId = Guid.NewGuid();

    private static IReadOnlyCollection<ServiceForRegister> CreateMockedServices()
    {
        var substitutedIdentityService = Substitute.For<IIdentityService>();
        substitutedIdentityService.IsAuthenticated.Returns(true);
        substitutedIdentityService.CurrentUserId.Returns(UserId);
        
        var mockServices = new List<ServiceForRegister>
        {
            new(typeof(IIdentityService), _ => substitutedIdentityService, ServiceLifetime.Scoped)
        };
        
        return mockServices;
    }

    [Fact]
    public async Task GetOrganizationById_OrganizationWithUsers_OrganizationReturnedWithValidUsersCount()
    {
        // Arrange
        var dbContext = GetDbContextService();
            
        var organization = SetupOrganization();
        
        var setupUserInfos = new List<UserInfo>
        {
            SetupUserInfo(UserId),
            SetupUserInfo(Guid.NewGuid()),
            SetupUserInfo(Guid.NewGuid())
        };
        
        var organizationUsers = new [] 
        { 
            SetupOrganizationUser(UserId, organization.Id, OrganizationUserRole.Admin), 
            SetupOrganizationUser(setupUserInfos[1].Id, organization.Id, OrganizationUserRole.Member),
            SetupOrganizationUser(setupUserInfos[2].Id, organization.Id, OrganizationUserRole.Member)
        };
        
        await dbContext.Set<Organization>().AddAsync(organization);
        await dbContext.Set<OrganizationUser>().AddRangeAsync(organizationUsers);
        await dbContext.Set<UserInfo>().AddRangeAsync(setupUserInfos);
        await dbContext.SaveChangesAsync();

        var getUrl = $"{ApiConstants.DefaultPrefix}/{ApiConstants.V1}/{OrganizationEndpoints.BaseResource}/{organization.Id}";
        
        // Act
        var response = await HttpClient.GetAsync(getUrl);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var organizationResponse = await response.Content.ReadFromJsonAsync<OrganizationWithInfoDto>();
        organizationResponse!.UsersCount.Should().Be(3);
        organizationResponse.Name.Should().Be(organization.Name);
        organizationResponse.Description.Should().Be(organization.Description);
    }

    [Fact]
    public async Task GetOrganizations_UserHasOrganizations_OrganizationsReturned()
    {
        // Arrange
        var dbContext = GetDbContextService();
        var firstOrganization = SetupOrganization();
        var secondOrganization = SetupOrganization();

        var userInfo = SetupUserInfo(UserId);
        
        var firstOrganizationUser = SetupOrganizationUser(UserId, firstOrganization.Id, OrganizationUserRole.Admin);
        var secondOrganizationUser = SetupOrganizationUser(UserId, secondOrganization.Id, OrganizationUserRole.Member);
        
        await dbContext.Set<Organization>().AddRangeAsync(firstOrganization, secondOrganization);
        await dbContext.Set<OrganizationUser>().AddRangeAsync(firstOrganizationUser, secondOrganizationUser);
        await dbContext.Set<UserInfo>().AddAsync(userInfo);
        await dbContext.SaveChangesAsync();
        
        var url = $"{ApiConstants.DefaultPrefix}/{ApiConstants.V1}/{OrganizationEndpoints.BaseResource}";
        
        // Act
        var response = await HttpClient.GetAsync(url);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var organizations = await response.Content.ReadFromJsonAsync<OrganizationDto[]>();
        organizations.Should().HaveCount(2);
        
        var responseFirstOrganization = organizations!.SingleOrDefault(x => x.Id == firstOrganization.Id);
        var responseSecondOrganization = organizations!.SingleOrDefault(x => x.Id == secondOrganization.Id);
        
        responseFirstOrganization.Should().NotBeNull();
        responseFirstOrganization!.Name.Should().Be(firstOrganization.Name);
        responseFirstOrganization.Description.Should().Be(firstOrganization.Description);

        responseSecondOrganization.Should().NotBeNull();
        responseSecondOrganization!.Name.Should().Be(secondOrganization.Name);
        responseSecondOrganization.Description.Should().Be(secondOrganization.Description);
    }
    
    [Fact]
    public async Task GetOrganizationUserRole_UserHasAdminRole_AdminRoleReturned()
    {
        // Arrange
        var dbContext = GetDbContextService();
        var firstOrganization = SetupOrganization();

        var userInfo = SetupUserInfo(UserId);
        
        var firstOrganizationUser = SetupOrganizationUser(UserId, firstOrganization.Id, OrganizationUserRole.Admin);
        
        await dbContext.Set<Organization>().AddRangeAsync(firstOrganization);
        await dbContext.Set<OrganizationUser>().AddRangeAsync(firstOrganizationUser);
        await dbContext.Set<UserInfo>().AddAsync(userInfo);
        await dbContext.SaveChangesAsync();
        
        var url = $"{ApiConstants.DefaultPrefix}/{ApiConstants.V1}/{OrganizationEndpoints.BaseResource}/" +
                  $"{firstOrganization.Id}/role";
        
        // Act
        var response = await HttpClient.GetAsync(url);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var userRoleResponse = await response.Content.ReadFromJsonAsync<GetOrganizationUserRoleResponse>(SetupJsonSerializerOptions());
        userRoleResponse!.Role.Should().Be(OrganizationUserRole.Admin);
    }
    
    [Fact]
    public async Task GetOrganizationUserRole_UserHasMemberRole_MemberRoleReturned()
    {
        // Arrange
        var dbContext = GetDbContextService();
        var firstOrganization = SetupOrganization();

        var userInfo = SetupUserInfo(UserId);
        
        var firstOrganizationUser = SetupOrganizationUser(UserId, firstOrganization.Id, OrganizationUserRole.Member);
        
        await dbContext.Set<Organization>().AddRangeAsync(firstOrganization);
        await dbContext.Set<OrganizationUser>().AddRangeAsync(firstOrganizationUser);
        await dbContext.Set<UserInfo>().AddAsync(userInfo);
        await dbContext.SaveChangesAsync();
        
        var url = $"{ApiConstants.DefaultPrefix}/{ApiConstants.V1}/{OrganizationEndpoints.BaseResource}/" +
                  $"{firstOrganization.Id}/role";
        
        // Act
        var response = await HttpClient.GetAsync(url);
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var userRoleResponse = await response.Content.ReadFromJsonAsync<GetOrganizationUserRoleResponse>(SetupJsonSerializerOptions());
        userRoleResponse!.Role.Should().Be(OrganizationUserRole.Member);
    }

    [Fact]
    public async Task CreateOrganization_UserCreateOrganization_OrganizationCreatedAndHasOneUserAsAdmin()
    {
        // Arrange
        var dbContext = GetDbContextService();
        
        var name = _faker.Random.AlphaNumeric(8);
        var description = _faker.Random.AlphaNumeric(32);
        var request = new CreateOrganizationRequest(name, description);
        var postUrl = $"{ApiConstants.DefaultPrefix}/{ApiConstants.V1}/{OrganizationEndpoints.BaseResource}";
        await CreateUserInDatabase(dbContext, UserId);
        
        // Act
        var response = await HttpClient.PostAsJsonAsync(postUrl, request);
        var returnedOrganizationId = await response.Content.ReadFromJsonAsync<Guid>();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var organization = await dbContext.Set<Organization>().SingleOrDefaultAsync();
        
        returnedOrganizationId.Should().Be(organization!.Id);
        organization.Should().NotBeNull();
        organization!.Name.Should().Be(name);
        organization.Description.Should().Be(description);
        
        var organizationUsers = await dbContext.Set<OrganizationUser>().ToListAsync();
        organizationUsers.Should().HaveCount(1);
        var organizationUser = organizationUsers[0];
        organizationUser.OrganizationId.Should().Be(organization.Id);
        organizationUser.UserId.Should().Be(UserId);
        organizationUser.Role.Should().Be(OrganizationUserRole.Admin);
    }
    
    private Organization SetupOrganization()
    {
        return new Organization
        {
            Id = Guid.NewGuid(),
            Name = _faker.Random.AlphaNumeric(8),
            Description = _faker.Random.AlphaNumeric(32)
        };
    }

    private UserInfo SetupUserInfo(Guid userId)
    {
        return new UserInfo()
        {
            Id = userId
        };
    }
    
    private OrganizationUser SetupOrganizationUser(
        Guid userId,
        Guid organizationId, 
        OrganizationUserRole role)
    {
        return new OrganizationUser
        {
            OrganizationId = organizationId,
            UserId = userId,
            Role = role
        };
    }
    
    private async Task CreateUserInDatabase(ApplicationDbContext applicationDbContext, Guid userId)
    {
        var user = new UserInfo
        {
            Id = userId
        };
        await applicationDbContext.Set<UserInfo>().AddAsync(user);
        await applicationDbContext.SaveChangesAsync();
    }

    private JsonSerializerOptions SetupJsonSerializerOptions()
    {
        return new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
    }
}