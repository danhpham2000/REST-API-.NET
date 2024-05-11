using DotNetScratch.Entities;
using Microsoft.EntityFrameworkCore;

namespace DotNetScratch.Data;

public class GameStoreContext(DbContextOptions<GameStoreContext> options) 
    : DbContext(options)
{
    // DbSet is object for query and save
    public DbSet<Game> Games => Set<Game>();
    public DbSet<Genre> Genres => Set<Genre>();
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Genre>().HasData(
                new {Id = 1, Name = "Fighting"},
                new {Id = 2, Name = "Fighting 2"},
                new {Id = 3, Name = "Fighting 3"},
                new {Id = 4, Name = "Fighting 4"},
                new {Id = 5, Name = "Fighting 5"}
            );
    }
}