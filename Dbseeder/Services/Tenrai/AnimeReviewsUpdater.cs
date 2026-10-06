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
using System.Formats.Tar;



namespace Dbseeder.Services.Updaters;

public class AnimeReviewsUpdater : AnimeUpdaterBase<TenraiAnimeReviewsResponseDto>
{
    public AnimeReviewsUpdater(ITenraiApiClient tenrai, MyDbContext db)
        : base(tenrai, db) {}

    public override async Task<TenraiAnimeReviewsResponseDto?> FetchDto(int id)
    {
        // Fetch first page
        var response = await _tenrai.GetAnimeReviewsAsync(id);
        return response;
    }

    public override async Task ApplyUpdate(int id, TenraiAnimeReviewsResponseDto dto)
    {
        var anime = await _db.Animes
                .Include(a => a.AnimeReviews)
                .FirstOrDefaultAsync(a => a.MalId == id);
        
        var allDtoIds = new HashSet<int>();
        var current = dto;
        if (anime == null)
            {
                // for now i will ony add anime then doing full anime search anything else will be ignored
                return;
            }

        var animeReviews = current.Data;    
        int page = 1;
        
        while (true)
        {
            foreach (var recDto in animeReviews)
            {
                // MAL recommended anime ID
                int reviewMalId = recDto.Mal_Id;
                allDtoIds.Add(reviewMalId);


                // Check if recommendation already exists
                var existing = anime.AnimeReviews
                    .FirstOrDefault(r => r.MalId == reviewMalId);

                if (existing == null)
                {
                    

                    // Create new recommendation
                    var entity = AnimeReviewsMapping.ToEntity(
                        anime,
                        recDto);

                    anime.AnimeReviews.Add(entity);
                    _db.AnimeReviews.Add(entity);
                }
                else
                {
                    AnimeReviewsMapping.UpdateEntity(existing, recDto);
                }
            }

            if (!current.Pagination.Has_Next_Page)
            {
                 break;
            }      
            page++;
            current = await _tenrai.GetAnimeReviewsAsync(id, page);

        }
        
        // Remove reviews not present in ANY page
        var toRemove = anime.AnimeReviews
            .Where(r => !allDtoIds.Contains(r.MalId))
            .ToList();

        foreach (var rem in toRemove)
        {
            anime.AnimeReviews.Remove(rem);
            _db.AnimeReviews.Remove(rem);
        }


    }



}
