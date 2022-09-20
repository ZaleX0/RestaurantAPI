using RestaurantAPI.Entities;

namespace RestaurantAPI;

public class RestaurantSeeder
{
    private readonly RestaurantDbContext _context;

    public RestaurantSeeder(RestaurantDbContext context)
    {
        _context = context;
    }

    public void Seed()
    {
        if (_context.Database.CanConnect())
        {
            if (!_context.Roles.Any())
            {
                var roles = GetRoles();
                _context.Roles.AddRange(roles);
                _context.SaveChanges();
            }

            if (!_context.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                _context.Restaurants.AddRange(restaurants);
                _context.SaveChanges();
            }
        }
    }


    private IEnumerable<Role> GetRoles()
    {
        return new List<Role>()
        {
            new Role()
            {
                Name = "User"
            },
            new Role()
            {
                Name = "Manager"
            },
            new Role()
            {
                Name = "Admin"
            }
        };
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        return new List<Restaurant>()
        {
            new Restaurant()
            {
                Name = "KFC",
                Category = "Fast Food",
                Description = "KFC Descirption",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,
                Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                        Name = "Kurczak",
                        Price = 10.30M
                    },
                    new Dish()
                    {
                        Name = "Burger",
                        Price = 5.30M
                    },
                },
                Address = new Address()
                {
                    City = "Kraków",
                    Street = "Długa 1",
                    PostalCode = "30-001"
                }
            },

            new Restaurant()
            {
                Name = "McDonalds",
                Category = "Fast Food",
                Description = "McDonalds Descirption",
                ContactEmail = "contact@mcdonalds.com",
                HasDelivery = false,
                Dishes = new List<Dish>()
                {
                    new Dish()
                    {
                        Name = "Hamburger",
                        Price = 4.00M
                    },
                    new Dish()
                    {
                        Name = "Frytki",
                        Price = 6.60M
                    },
                },
                Address = new Address()
                {
                    City = "Kraków",
                    Street = "Długa 10",
                    PostalCode = "30-001"
                }
            }
        };
    }
}
