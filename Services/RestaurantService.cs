using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Threading;

namespace RestaurantAPI.Services;

public class RestaurantService : IRestaurantService
{
    private readonly RestaurantDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<RestaurantService> _logger;

    public RestaurantService(RestaurantDbContext context, IMapper mapper, ILogger<RestaurantService> logger)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<RestaurantDto?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var restaurant = await _context
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
        return restaurantDto;
    }

    public async Task<IEnumerable<RestaurantDto>> GetAllAsync(CancellationToken cancellationToken)
    {
        var restaurants = await _context
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .ToListAsync(cancellationToken);

        var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);
        return restaurantsDtos;
    }

    public async Task<int> CreateAsync(CreateRestaurantDto dto, CancellationToken cancellationToken)
    {
        var restaurant = _mapper.Map<Restaurant>(dto);
        await _context.Restaurants.AddAsync(restaurant, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return restaurant.Id;
    }

    public async Task UpdateAsync(int id, UpdateRestaurantDto dto, CancellationToken cancellationToken)
    {
        var restaurant = await _context
            .Restaurants
            .FirstOrDefaultAsync(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        restaurant.Name = dto.Name;
        restaurant.Description = dto.Description;
        restaurant.HasDelivery = dto.HasDelivery;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken)
    {
        _logger.LogError($"Restaurant with id: {id} DELETE action invoked");

        var restaurant = await _context
            .Restaurants
            .FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        _context.Restaurants.Remove(restaurant);
        await _context.SaveChangesAsync(cancellationToken);
    }

}
