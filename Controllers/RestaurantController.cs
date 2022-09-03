using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Controllers;

[ApiController]
[Route("api/restaurant")]
public class RestaurantController : ControllerBase
{
    private readonly RestaurantDbContext _context;

    public RestaurantController(RestaurantDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Restaurant>>> GetAll(CancellationToken cancellationToken)
    {
        var restaurants = await _context.Restaurants
            .ToListAsync(cancellationToken);

        return Ok(restaurants);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Restaurant>> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var restaurant = await _context.Restaurants
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        return Ok(restaurant);
    }
}
