using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[ApiController]
[Route("api/restaurant")]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _service;

    public RestaurantController(IRestaurantService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<ActionResult> CreateRestaurant([FromBody] CreateRestaurantDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var id = await _service.CreateAsync(dto, cancellationToken);

        return Created($"/api/restaurant/{id}", null);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Restaurant>>> GetAll(CancellationToken cancellationToken)
    {
        var restaurantsDtos = await _service.GetAllAsync(cancellationToken);

        return Ok(restaurantsDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Restaurant>> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var restaurant = await _service.GetByIdAsync(id);

        if (restaurant is null)
            return NotFound();

        return Ok(restaurant);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update([FromRoute] int id, UpdateRestaurantDto dto, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        var isUpdated = await _service.UpdateAsync(id, dto, cancellationToken);

        if (!isUpdated)
            return NotFound();

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var isDeleted = await _service.Delete(id, cancellationToken);

        if (isDeleted)
            return NoContent();

        return NotFound();
    }
}
