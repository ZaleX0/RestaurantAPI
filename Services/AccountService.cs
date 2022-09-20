using Microsoft.AspNetCore.Identity;
using Microsoft.JSInterop.Infrastructure;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public interface IAccountService
{
    void RegisterUser(RegisterUserDto dto);
}

public class AccountService : IAccountService
{
    private readonly RestaurantDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountService(RestaurantDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public void RegisterUser(RegisterUserDto dto)
    {
        var newUser = new User()
        {
            Email = dto.Email,
            Nationality = dto.Nationality,
            DateOfBirth = dto.DateOfBirth,
            RoleId = dto.RoleId,
        };
        var hash = _passwordHasher.HashPassword(newUser, dto.Password);
        newUser.PasswordHash = hash;

        _context.Users.Add(newUser);
        _context.SaveChanges();
    }
}
