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

public class AnimeStaffUpdater : AnimeUpdaterBase<TenraiAnimeStaffResponseDto>
{
    public AnimeStaffUpdater(ITenraiApiClient tenrai, MyDbContext db)
        : base(tenrai, db) {}

    public override async Task<TenraiAnimeStaffResponseDto?> FetchDto(int id)
    {
        // Fetch first page
        var response = await _tenrai.GetAnimeStaffAsync(id);
        return response;
    }
    public override async Task ApplyUpdate(int id, TenraiAnimeStaffResponseDto dto)
    {


        var staffDtos = dto.Data;

        var anime = await _db.Animes
                .Include(a => a.Staff)
                .ThenInclude(a => a.Person)
                .FirstOrDefaultAsync(a => a.MalId == id);

        if (anime == null)
        {
            // for now i will ony add anime then doing full anime search anything else will be ignored
            return;
        }

        foreach (var staffDto in staffDtos)
        {
            int personMalId = staffDto.Person.Mal_Id;

            var person = await _db.Persons
                .FirstOrDefaultAsync(p => p.MalId == personMalId);

            if (person == null)
            {
                person = new Person
                {
                    MalId = personMalId,
                    Name = staffDto.Person.Name,
                    ImageUrl = staffDto.Person.Images?.Jpg?.Image_Url
                };

                _db.Persons.Add(person);
            }

            // For each position → create a separate AnimeStaff row
            foreach (var position in staffDto.Positions)
            {
                var existing = anime.Staff
                    .FirstOrDefault(s =>
                        s.Person.MalId == personMalId &&
                        s.Position == position);

                if (existing == null)
                {
                    var entity = AnimeStaffMapping.ToEntity(anime, person, position);
                    anime.Staff.Add(entity);
                    _db.AnimeStaff.Add(entity);
                }
            }
        }

        // Remove staff rows no longer present
        var dtoPairs = staffDtos
            .SelectMany(s => s.Positions.Select(pos => (s.Person.Mal_Id, pos)))
            .ToHashSet();

        var toRemove = anime.Staff
            .Where(s => !dtoPairs.Contains((s.Person.MalId, s.Position)))
            .ToList();

        foreach (var rem in toRemove)
        {
            anime.Staff.Remove(rem);
            _db.AnimeStaff.Remove(rem);
        }

    }

}
