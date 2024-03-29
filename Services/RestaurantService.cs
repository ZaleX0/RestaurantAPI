﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Linq.Expressions;

namespace RestaurantAPI.Services;

public interface IRestaurantService
{
    int Create(CreateRestaurantDto dto);
    PagedResult<RestaurantDto> GetAll(RestaurantQuery query);
    RestaurantDto? GetById(int id);
    void Update(int id, UpdateRestaurantDto dto);
    void Delete(int id);
}

public class RestaurantService : IRestaurantService
{
    private readonly RestaurantDbContext _context;
    private readonly IMapper _mapper;
    private readonly ILogger<RestaurantService> _logger;
    private readonly IAuthorizationService _authorizationService;
    private readonly IUserContextService _userContextService;

    public RestaurantService(
        RestaurantDbContext context,
        IMapper mapper,
        ILogger<RestaurantService> logger,
        IAuthorizationService authorizationService,
        IUserContextService userContextService)
    {
        _context = context;
        _mapper = mapper;
        _logger = logger;
        _authorizationService = authorizationService;
        _userContextService = userContextService;
    }

    public RestaurantDto? GetById(int id)
    {
        var restaurant = _context
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");

        var restaurantDto = _mapper.Map<RestaurantDto>(restaurant);
        return restaurantDto;
    }

    public PagedResult<RestaurantDto> GetAll(RestaurantQuery query)
    {
        var baseQuery = _context
            .Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .Where(r => string.IsNullOrEmpty(query.SearchPhrase)
                || r.Name.ToLower().Contains(query.SearchPhrase.ToLower())
                || r.Description.ToLower().Contains(query.SearchPhrase.ToLower())
            );



        if (!string.IsNullOrEmpty(query.SortBy))
        {
            var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
            {
                { nameof(Restaurant.Name), r => r.Name },
                { nameof(Restaurant.Description), r => r.Description },
                { nameof(Restaurant.Category), r => r.Category }
            };

            var selectedColumn = columnsSelector[query.SortBy];

            baseQuery = query.SortDirection == SortDirection.DESC
                ? baseQuery.OrderByDescending(selectedColumn)
                : baseQuery.OrderBy(selectedColumn);
        }



        var restaurants = baseQuery
            .Skip(query.PageSize * (query.PageNumber - 1))
            .Take(query.PageSize)
            .ToList();




        var totalItemsCount = baseQuery.Count();

        var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);

        var result = new PagedResult<RestaurantDto>(restaurantsDtos, totalItemsCount, query.PageSize, query.PageNumber);

        return result;
    }

    public int Create(CreateRestaurantDto dto)
    {
        var restaurant = _mapper.Map<Restaurant>(dto);
        restaurant.CreatedById = _userContextService.GetUserId;
        _context.Restaurants.Add(restaurant);
        _context.SaveChanges();
        return restaurant.Id;
    }

    public void Update(int id, UpdateRestaurantDto dto)
    {
        var restaurant = _context
            .Restaurants
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");



        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
            new ResourceOperationRequirement(ResourceOperation.Update)).Result;

        if (!authorizationResult.Succeeded)
            throw new ForbidException();



        restaurant.Name = dto.Name;
        restaurant.Description = dto.Description;
        restaurant.HasDelivery = dto.HasDelivery;

        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        _logger.LogError($"Restaurant with id: {id} DELETE action invoked");

        var restaurant = _context
            .Restaurants
            .FirstOrDefault(r => r.Id == id);

        if (restaurant is null)
            throw new NotFoundException("Restaurant not found");



        var authorizationResult = _authorizationService.AuthorizeAsync(_userContextService.User, restaurant,
            new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

        if (!authorizationResult.Succeeded)
            throw new ForbidException();



        _context.Restaurants.Remove(restaurant);
        _context.SaveChanges();
    }

}
