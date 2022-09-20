using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/restaurant/{restaurantId}/dish")]
[ApiController]
public class DishController : ControllerBase
{
    private readonly IDishService _service;

    public DishController(IDishService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
    {
        var newDishId = _service.Create(restaurantId, dto);
        return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
    }


    [HttpDelete]
    public IActionResult Delete([FromRoute] int restaurantId)
    {
        _service.RemoveAll(restaurantId);
        return NoContent();
    }

    [HttpGet("{dishId}")]
    public IActionResult Get([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dish = _service.GetById(restaurantId, dishId);
        return Ok(dish);
    }

    [HttpGet]
    public IActionResult GetAll([FromRoute] int restaurantId)
    {
        var dishes = _service.GetAll(restaurantId);
        return Ok(dishes);
    }
}
