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

public class AnimeEpisodesUpdater : AnimeUpdaterBase<TenraiAnimeEpisodesResponseDto>
{
    public AnimeEpisodesUpdater(ITenraiApiClient tenrai, MyDbContext db)
        : base(tenrai, db) {}

    public override async Task<TenraiAnimeEpisodesResponseDto?> FetchDto(int id)
    {
        // Fetch first page
        var response = await _tenrai.GetAnimeEpisodesAsync(id);
        return response;
    }

    public override async Task ApplyUpdate(int id, TenraiAnimeEpisodesResponseDto dto)
    {
        Console.WriteLine($"Updating Episodes for  anime {id}");
        var anime = await _db.Animes
                .Include(a => a.AnimeEpisodes)
                .FirstOrDefaultAsync(a => a.MalId == id);
        
        var allDtoIds = new HashSet<int>();
        var current = dto;
        if (anime == null)
            {
                // for now i will ony add anime then doing full anime search anything else will be ignored
                return;
            }

        var animeEpisodes = current.Data;    
        int page = 1;
        
        while (true)
        {
            foreach (var recDto in animeEpisodes)
            {
                // MAL recommended anime ID
                int episodeId = recDto.Mal_Id;
                allDtoIds.Add(episodeId);


                // Check if recommendation already exists
                var existing = anime.AnimeEpisodes
                    .FirstOrDefault(e => e.EpisodeNumber == episodeId);

                if (existing == null)
                {
                    

                    // Create new recommendation
                    var entity = AnimeEpisodesMapping.ToEntity(
                        anime,
                        recDto);

                    anime.AnimeEpisodes.Add(entity);
                    _db.AnimeEpisodes.Add(entity);
                }
                else
                {
                    AnimeEpisodesMapping.UpdateEntity(existing, recDto);
                }
            }

            if (!current.Pagination.Has_Next_Page)
            {
                 break;
            }      
            page++;
            current = await _tenrai.GetAnimeEpisodesAsync(id, page);

        }
        
        // Remove reviews not present in ANY page
        var toRemove = anime.AnimeEpisodes
            .Where(r => !allDtoIds.Contains(r.EpisodeNumber))
            .ToList();

        foreach (var rem in toRemove)
        {
            anime.AnimeEpisodes.Remove(rem);
            _db.AnimeEpisodes.Remove(rem);
        }


    }



}
