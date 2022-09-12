using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant")]
[ApiController]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _service;

    public RestaurantController(IRestaurantService service)
    {
        _service = service;
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant([FromBody] CreateRestaurantDto dto, CancellationToken cancellationToken)
    {
        var id = await _service.CreateAsync(dto, cancellationToken);

        return Created($"/api/restaurant/{id}", null);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var restaurantsDtos = await _service.GetAllAsync(cancellationToken);

        return Ok(restaurantsDtos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get([FromRoute] int id, CancellationToken cancellationToken)
    {
        var restaurant = await _service.GetByIdAsync(id);

        return Ok(restaurant);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, UpdateRestaurantDto dto, CancellationToken cancellationToken)
    {
        await _service.UpdateAsync(id, dto, cancellationToken);

        return Ok();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        await _service.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}
