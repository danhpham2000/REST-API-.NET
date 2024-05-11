using DotNetScratch.Data;
using DotNetScratch.Dtos;
using DotNetScratch.Entities;
using DotNetScratch.Mapping;
using Microsoft.EntityFrameworkCore;

namespace DotNetScratch.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndPointName = "GetGame";
    
    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {

        var group = app.MapGroup("games").WithParameterValidation();
        
        // GET /games
        // Transform the asynchronous programming
        group.MapGet("/", async (GameStoreContext dbContext) => 
            await dbContext.Games
                .Include(game => game.Genre)
                .Select(game => game.ToGameSummaryDto())
                .AsNoTracking()
                .ToListAsync());

        // GET /games/:id
        group.MapGet("/{id}", async (int id, GameStoreContext dbContext) =>
            {
                Game? game = await dbContext.Games.FindAsync(id);
                return game is null ? 
                    Results.NotFound() : Results.Ok(game.ToGameDetailsDto());
            })
            .WithName(GetGameEndPointName);

        // POST /games
        group.MapPost("/", async (CreateGameDto newGame, 
                GameStoreContext dbContext) =>
            {
                Game game = newGame.ToEntity();
                dbContext.Games.Add(game);
                await dbContext.SaveChangesAsync();
                
            return Results.CreatedAtRoute(
                GetGameEndPointName, new {id = game.Id}, game.ToGameDetailsDto());
        })
        .WithParameterValidation();

        // PUT /games
        group.MapPut("/{id}", async (int id, UpdateGameDto updateGame, GameStoreContext dbContext) =>
        {
            var existingGame = await dbContext.Games.FindAsync(id);
            if (existingGame == null)
            {
                return Results.NotFound();
            }

            dbContext.Entry(existingGame)
                .CurrentValues.SetValues(updateGame.ToEntity(id));

            await dbContext.SaveChangesAsync();
            
            return Results.NoContent();
        });

        group.MapDelete("/{id}", async (int id, GameStoreContext dbContext) =>
        {
            await dbContext.Games
                .Where(game => game.Id == id)
                .ExecuteDeleteAsync();
            return Results.NoContent();
        });

        return group;
    }
}