using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization;

public class ResourceOperationRequirementHandler : AuthorizationHandler<ResourceOperationRequirement, Restaurant>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        ResourceOperationRequirement requirement,
        Restaurant restaurant)
    {
        if (requirement.ResourceOperation == ResourceOperation.Read ||
            requirement.ResourceOperation == ResourceOperation.Create)
        {
            context.Succeed(requirement);
        }

        var userIdClaim = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim is null)
            return Task.CompletedTask;
        if (restaurant.CreatedById == int.Parse(userIdClaim.Value))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
