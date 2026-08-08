using Api.Tenrai;
using MyEfModels.Data;
using MyEfModels.Entities;
using Microsoft.EntityFrameworkCore;
using MyEfModels.Mapping;
using System.Text.Json;
using Api.Tenrai.Dtos.Anime;
using MyEfModels.Mapping.Tenrai;
using System.Runtime.Intrinsics.X86;
using System.Data.Common;
using Api.Mal.Dtos.AnimeDetails;
using System.IO.Compression;

namespace Dbseeder.Services;

public class PersonDetailsService
{
    private readonly ITenraiApiClient _tenrai;
    private readonly MyDbContext _db;

    public PersonDetailsService(ITenraiApiClient tenrai, MyDbContext db )
    {
        _tenrai = tenrai;
        _db = db;
        
    }

    public async Task UpdatePersonDetails(string username, string? startDate = null , string? status = null )
    {
        var userId = await _db.Users
            .Where(u => u.Username == username)
            .Select(u => (int?)u.Id)
            .FirstOrDefaultAsync();

        if (userId == null)
        {
            Console.WriteLine ($"User with Username {username} is not in the db");
            return;
        } 

        var query = _db.UserList
            .Where(ul => ul.UserId == userId);



        if (!string.IsNullOrEmpty(startDate))
        {
            query = query.Where(ul => ul.StartDate != null &&
                          ul.StartDate.CompareTo(startDate) >= 0);

        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(ul => ul.Status == status);
        }

        var vaids = await query
            .SelectMany(ul => ul.Anime.AnimeCharacters)
            .SelectMany(ac => ac.VoiceActors)
            .Select(va => va.Person.MalId)
            .ToHashSetAsync();


        var staffids = await query
            .SelectMany(ul => ul.Anime.Staff)
            .Select(s => s.Person.MalId)
            .ToHashSetAsync();

        var ids = vaids.Union(staffids).ToList();



        await UpdatePersonDetails(ids);

    }

    public async Task UpdatePersonDetails(List<int> ids)
    {
        
        
        foreach (int id in ids)
        {
            await Task.Delay(1000);

            var response = await _tenrai.GetPersonDetails(id);

            if (response == null)
            {
                Console.WriteLine($"❌ Tenrai returned NULL for id {id}");
                break; // or continue; depending on your preference
            }

            if (response.Data == null)
            {
                Console.WriteLine($"❌ MAL returned NULL Data for id {id}");
                break;
            }
            var personDto = response.Data;

            var person = await _db.Persons
                .Include(p => p.AnimeStaffRoles)
                    .ThenInclude(s => s.Anime)
                .Include(p => p.VoiceActingRoles)
                    .ThenInclude(va => va.AnimeCharacter)
                        .ThenInclude(ac => ac.Character)   // ⭐ THIS loads AnimeCharacter.Character
                .Include(p => p.VoiceActingRoles)
                    .ThenInclude(va => va.AnimeCharacter)
                        .ThenInclude(ac => ac.Anime)
                .FirstOrDefaultAsync(c => c.MalId == id);



            if (person == null)
            {
                person = TenraiPersonDetailsMapping.ToEntity(personDto);
                _db.Persons.Add(person);
            }
            else
            {
                TenraiPersonDetailsMapping.UpdateEntity(person, personDto);
            }

            foreach (var animeDto in personDto.Anime)
            {
                var personRole = person.AnimeStaffRoles
                    .FirstOrDefault(p => p.Anime.MalId == animeDto.Anime.Mal_Id && p.Position == animeDto.Position);
                    
                if (personRole == null)
                {
                    var anime = _db.Animes.FirstOrDefault(a => a.MalId == animeDto.Anime.Mal_Id);

                    if (anime == null)
                    {
                        // For now anime will only be added through full update
                        continue;
                    }
                    else
                    {
                        personRole = TenraiPersonDetailsMapping.ToAnimeStaffEntity(person, anime, animeDto.Position);
                        person.AnimeStaffRoles.Add(personRole);
                        _db.AnimeStaff.Add(personRole);
                    }
                }

                // VA roles will not be added because PersonDetails endpit doesn't provide the language of the role

            }

            var tenraiRoles = personDto.Anime
                .Select(a => new { AnimeMalId = a.Anime.Mal_Id, Position = a.Position })
                .ToList();

            
            var rolesToDelete = person.AnimeStaffRoles
                .Where(dbRole => !tenraiRoles.Any(tr =>
                    tr.AnimeMalId == dbRole.Anime.MalId &&
                    tr.Position == dbRole.Position))
                .ToList();

            foreach (var role in rolesToDelete)
            {
                person.AnimeStaffRoles.Remove(role);
                _db.AnimeStaff.Remove(role);
            }  
            await _db.SaveChangesAsync();
        }
    }



}
