using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;
using System.Security.Claims;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant")]
[ApiController]
[Authorize]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _service;

    public RestaurantController(IRestaurantService service)
    {
        _service = service;
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
    {
        var id = _service.Create(dto);
        return Created($"/api/restaurant/{id}", null);
    }

    [HttpGet]
    //[Authorize(Policy = "CreatedAtleast2Restaurants")]
    public IActionResult GetAll([FromQuery] RestaurantQuery query)
    {
        var restaurantsDtos = _service.GetAll(query);
        return Ok(restaurantsDtos);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public IActionResult Get([FromRoute] int id)
    {
        var restaurant = _service.GetById(id);
        return Ok(restaurant);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Update([FromRoute] int id, UpdateRestaurantDto dto)
    {
        _service.Update(id, dto);
        return Ok();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public IActionResult Delete([FromRoute] int id)
    {
        _service.Delete(id);
        return NoContent();
    }
}
