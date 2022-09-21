using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController : ControllerBase
{
    private readonly IAccountService _service;

    public AccountController(IAccountService service)
    {
        _service = service;
    }

    [HttpPost("register")]
    public IActionResult RegisterUser([FromBody] RegisterUserDto dto)
    {
        _service.RegisterUser(dto);
        return Ok();
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto dto)
    {
        string token = _service.GenerateJwt(dto);
        return Ok(token);
    }

    // tmp
    [HttpGet]
    public IActionResult GetUsers()
    {
        var users = _service.GetUsers();
        return Ok(users);
    }
}
