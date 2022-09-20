﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace RestaurantAPI.Entities;

public class RestaurantDbContext : DbContext
{
    private readonly string _connectionString = "Data Source=RestaurantDb.db";

    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Dish> Dishes { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
            .Property(u => u.Email)
            .IsRequired();

        modelBuilder.Entity<Role>()
            .Property(u => u.Name)
            .IsRequired();

        modelBuilder.Entity<Restaurant>()
            .Property(r => r.Name)
            .IsRequired()
            .HasMaxLength(25);

        modelBuilder.Entity<Dish>()
            .Property(d => d.Name)
            .IsRequired();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite(_connectionString);
    }
}
