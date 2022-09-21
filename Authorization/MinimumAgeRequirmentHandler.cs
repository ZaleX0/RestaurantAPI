using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;
using System.Net.Mime;
using System.Security.Claims;

namespace RestaurantAPI.Authorization;

public class MinimumAgeRequirmentHandler : AuthorizationHandler<MinimumAgeRequirment>
{
    private readonly ILogger<MinimumAgeRequirmentHandler> _logger;

    public MinimumAgeRequirmentHandler(ILogger<MinimumAgeRequirmentHandler> logger)
    {
        _logger = logger;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirment requirement)
    {
        var userDateOfBirthClaim = context.User.FindFirst(c => c.Type == "DateOfBirth");
        if (userDateOfBirthClaim is null)
            return Task.CompletedTask;

        var dateOfBirth = DateTime.Parse(userDateOfBirthClaim.Value);

        var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name).Value;
        _logger.LogInformation($"User: {userEmail} with date of birth: [{dateOfBirth}]");

        if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
        {
            _logger.LogInformation("Authorization succeded");
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogInformation("Authorization failed");
        }

        return Task.CompletedTask;
    }
}
