using System.ComponentModel.DataAnnotations;

namespace DotNetScratch.Dtos;

public record class UpdateGameDto(
    [Required][StringLength(50)]
    string Name, 
    int GenreId, 
    [Required][Range(0, 100)]
    decimal Price, 
    DateOnly ReleaseDate);