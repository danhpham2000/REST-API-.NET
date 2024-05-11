using DotNetScratch.Dtos;
using DotNetScratch.Entities;

namespace DotNetScratch.Mapping;

public static class GenreMapping
{
    public static GenreDto ToDto(this Genre genre)
    {
        return new GenreDto(genre.Id, genre.Name);
    }
    
}