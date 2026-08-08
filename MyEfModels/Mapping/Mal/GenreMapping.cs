using MyEfModels.Entities;
using Api.Mal.Dtos;
using Api.Mal.Dtos.AnimeRanking;



namespace MyEfModels.Mapping;

public static class GenreMappingExtensions
{
    public static Genre ToEntity(this GenreDto dto)
    {
        return new Genre
        {
            MalId = dto.Id,       // Store MAL ID separately if needed
            Name = dto.Name,      // Required
        };
    }



}

