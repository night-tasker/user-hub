﻿namespace NightTasker.UserHub.Presentation.WebApi.Endpoints;

public static class OrganizationEndpoints
{
    public const string BaseResource = "organizations";
    
    public const string GetById = "{organizationId}";

    public const string GetUserOrganizationRole = "{organizationId}/role";
    
    public const string SearchOrganizations = "search";
    
    public const string UpdateOrganization = "{organizationId}";
    
    public const string DeleteOrganization = "{organizationId}";
}