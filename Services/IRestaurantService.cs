using RestaurantAPI.Entities;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;
public interface IRestaurantService
{
    Task<int> CreateAsync(CreateRestaurantDto dto, CancellationToken cancellationToken = default);
    Task<IEnumerable<RestaurantDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<RestaurantDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateRestaurantDto dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}