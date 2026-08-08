using Api.Tenrai;
using MyEfModels.Data;
using MyEfModels.Entities;
using Microsoft.EntityFrameworkCore;
using MyEfModels.Mapping;
using System.Text.Json;
using Api.Tenrai.Dtos.Anime;
using MyEfModels.Mapping.Tenrai;
using Api.Tenrai.Dtos.Response;
using Dbseeder.Services.Shared;



namespace Dbseeder.Services.Updaters;

public class AnimeRecommendationUpdater : AnimeUpdaterBase<TenraiAnimeRecommendationResponseDto>
{
    public AnimeRecommendationUpdater(ITenraiApiClient tenrai, MyDbContext db)
        : base(tenrai, db) {}

    public override async Task<TenraiAnimeRecommendationResponseDto?> FetchDto(int id)
    {
        // Fetch first page
        var response = await _tenrai.GetAnimeRecommendationAsync(id);
        return response;
    }

    public override async Task ApplyUpdate(int id, TenraiAnimeRecommendationResponseDto dto)
    {
        var anime = await _db.Animes
                .Include(a => a.RecommendationsFrom)
                .ThenInclude(a => a.Anime2)
                .FirstOrDefaultAsync(a => a.MalId == id);


        if (anime == null)
            {
                // for now i will ony add anime then doing full anime search anything else will be ignored
                return;
            }

        var animeRecommendations = dto.Data;    

        foreach (var recDto in animeRecommendations)
            {
                // MAL recommended anime ID
                int recommendedMalId = recDto.Entry.Mal_Id;


                // Check if recommendation already exists
                var existing = anime.RecommendationsFrom
                    .FirstOrDefault(r => r.Anime2?.MalId == recommendedMalId);

                if (existing == null)
                {
                    var recommendedAnime = await _db.Animes
                        .FirstOrDefaultAsync(a => a.MalId == recommendedMalId);

                    if (recommendedAnime == null)
                    {
                        continue;
                    
                    }

                    // Create new recommendation
                    var entity = AnimeRecommendationMapping.ToEntity(
                        recDto,
                        anime,
                        recommendedAnime);

                    anime.RecommendationsFrom.Add(entity);
                    _db.AnimeRecommendations.Add(entity);
                }
                else
                {
                    existing.Votes = recDto.Votes;
                }
            }

            // Remove recommendations no longer present in the DTO
            var dtoMalIds = animeRecommendations
                .Select(r => r.Entry.Mal_Id)
                .ToHashSet();

            var toRemove = anime.RecommendationsFrom
                .Where(r => !dtoMalIds.Contains(r.Anime2Id))
                .ToList();

            foreach (var rem in toRemove)
            {
                anime.RecommendationsFrom.Remove(rem);
                _db.AnimeRecommendations.Remove(rem);
            }

    }



}
