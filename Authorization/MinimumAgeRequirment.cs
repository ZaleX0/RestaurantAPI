using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace RestaurantAPI.Authorization;

public class MinimumAgeRequirment : IAuthorizationRequirement
{
    public int MinimumAge { get; }

	public MinimumAgeRequirment(int minimumAge)
	{
		MinimumAge = minimumAge;
	}
}
