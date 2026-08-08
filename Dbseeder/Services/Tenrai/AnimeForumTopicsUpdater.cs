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

public class AnimeForumTopicsUpdater : AnimeUpdaterBase<TenraiAnimeForumResponseDto>
{
    public AnimeForumTopicsUpdater(ITenraiApiClient tenrai, MyDbContext db)
        : base(tenrai, db) {}

    public override async Task<TenraiAnimeForumResponseDto?> FetchDto(int id)
    {
        // Fetch first page
        var response = await _tenrai.GetAnimeForumTopics(id);
        return response; // may be null → orchestrator handles it
    }

    public override async Task ApplyUpdate(int id, TenraiAnimeForumResponseDto dto)
    {
        int counter = 0;
        var current = dto;
        int page = 1;

        // Count first page
        if (current.Data != null)
            counter += current.Data.Count;

        // Fetch next pages
        while (current.Pagination?.Has_Next_Page == true)
        {
            page++;

            var next = await _tenrai.GetAnimeForumTopics(id, page);
            if (next?.Data == null)
                break;

            counter += next.Data.Count;
            current = next;
        }

        // Update DB
        await _db.Animes
            .Where(a => a.MalId == id)
            .ExecuteUpdateAsync(setters =>
                setters.SetProperty(a => a.forumTopicsCounter, counter));
    }
}
