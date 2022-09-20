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
    public IActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
    {
        var id = _service.Create(dto);

        return Created($"/api/restaurant/{id}", null);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var restaurantsDtos = _service.GetAll();

        return Ok(restaurantsDtos);
    }

    [HttpGet("{id}")]
    public IActionResult Get([FromRoute] int id)
    {
        var restaurant = _service.GetById(id);

        return Ok(restaurant);
    }

    [HttpPut("{id}")]
    public IActionResult Update([FromRoute] int id, UpdateRestaurantDto dto)
    {
        _service.Update(id, dto);

        return Ok();
    }

    [HttpDelete("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        _service.Delete(id);

        return NoContent();
    }
}
