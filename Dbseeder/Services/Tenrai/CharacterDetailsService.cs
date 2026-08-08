using Api.Tenrai;
using MyEfModels.Data;
using MyEfModels.Entities;
using Microsoft.EntityFrameworkCore;
using MyEfModels.Mapping;
using System.Text.Json;
using Api.Tenrai.Dtos.Anime;
using MyEfModels.Mapping.Tenrai;
using System.Runtime.Intrinsics.X86;

namespace Dbseeder.Services;

public class CharacterDetailsService
{
    private readonly ITenraiApiClient _tenrai;
    private readonly MyDbContext _db;
    public CharacterDetailsService(ITenraiApiClient tenrai, MyDbContext db )
    {
        _tenrai = tenrai;
        _db = db;
        
    }

    public async Task UpdateCharacterDetails(string username, string? startDate = null , string? status = null )
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

        var ids = await query
            .SelectMany(ul => ul.Anime.AnimeCharacters)
            .Select(ac => ac.Character.MalId)
            .ToListAsync();

        await UpdateCharacterDetails(ids);

    }

    public async Task UpdateCharacterDetails(List<int> ids)
    {
        
        
        foreach (int id in ids)
        {
            await Task.Delay(1000);

            var response = await _tenrai.GetCharacterDetails(id);

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
            var characterDto = response.Data;

            var character = await _db.Characters
                .Include(c => c.AnimeCharacters)
                    .ThenInclude(ac => ac.Anime)
                .FirstOrDefaultAsync(c => c.MalId == id);


            if (character == null)
            {
                character = TenraiCharacterDetailsMapping.ToEntity(characterDto);
                _db.Characters.Add(character);
            }
            else
            {
                TenraiCharacterDetailsMapping.UpdateEntity(character, characterDto);
            }

            // for now i will not add anime relations


            
            await _db.SaveChangesAsync();
        }
    }



}
