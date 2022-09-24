using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RestaurantAPI.Entities;
using System.Security.Claims;

namespace RestaurantAPI.Authorization;

public class CreatedMultipleRestaurantsRequirementHandler : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{
    private readonly RestaurantDbContext _dbContext;

    public CreatedMultipleRestaurantsRequirementHandler(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
    {
        var userIdClaim = context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier);
        if (userIdClaim is null)
            return Task.CompletedTask;

        var userId = int.Parse(userIdClaim.Value);
        var createdRestaurantsCount = _dbContext.Restaurants.Count(r => r.CreatedById == userId);

        if (createdRestaurantsCount > requirement.MinimumRestaurantsCreated)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
