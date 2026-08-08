using MyEfModels.Entities;
using Api.Mal.Dtos;
using Api.Mal.Dtos.AnimeRanking;


namespace MyEfModels.Mapping;

public static class StudioMappingExtensions
{
    public static Company ToEntity(this StudioDto dto)
    {
        return new Company
        {
            MalId = dto.Id,       // Store MAL ID separately if needed
            Name = dto.Name,      // Required
        };
    }



}

