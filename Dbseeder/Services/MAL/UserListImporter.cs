using Api.Mal;
using Api.Mal.Dtos.AnimeRanking;
using MyEfModels.Data;
using MyEfModels.Entities;
using Microsoft.EntityFrameworkCore;
using MyEfModels.Mapping;
using System.Text.Json;

namespace Dbseeder.Services;

public class UserListImporter
{
    private readonly IMalApiClient _mal;
    private readonly MyDbContext _db;

    public UserListImporter(IMalApiClient mal, MyDbContext db)
    {
        _mal = mal;
        _db = db;
        
    }

    public async Task ImportUserListAsync(string statusType, string username)
    {
        Console.WriteLine($"Importing ranking: {statusType}");

        int offset = 0;
        const int batchSize = 500;


        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Username == username);

        
        if (user == null)
        {
            user = new User { Username = username };
            _db.Users.Add(user);

        }


        await _db.SaveChangesAsync();
        while (true)
        {
            var response = await _mal.GetUserAnimeListAsync(username, limit: batchSize, offset: offset);


            if (response == null)
            {
                Console.WriteLine($"❌ MAL returned NULL at offset {offset} (JSON parse failed)");
                return;
            }

            if (response.Data == null)
            {
                Console.WriteLine($"❌ MAL returned NULL Data at offset {offset}");
                return;
            }

            if (response?.Data == null || response.Data.Count == 0)
                break;

            Console.WriteLine($"Fetched {response.Data.Count} items at offset {offset}");

            var items = response.Data;


            foreach (var item in response.Data)
            {
                var id = item.Node.Id;

                // Load anime + rankings
                var userListEntry = await _db.UserList
                    .Include(ul => ul.Anime)
                    .FirstOrDefaultAsync(ul => ul.UserId == user.Id && ul.Anime!.MalId == id);

                if (userListEntry == null)
                {
                    var anime = _db.Animes.FirstOrDefault(a => a.MalId == id);

                    // for now i not be adding anime here
                    if (anime == null)
                    {
                        continue;
                    }

                    // NEw ListEntries
                    userListEntry = item.ToEntity(anime, user);
                    try
                    {
                        _db.UserList.Add(userListEntry);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ Failed to add anime:");
                        Console.WriteLine(JsonSerializer.Serialize(
                                    anime,
                                    new JsonSerializerOptions { WriteIndented = true }
                                ));
                        Console.WriteLine($"Error: {ex.Message}");

                        return;
                    }
                    
                }
                else
                {
                    item.UpdateEntity(userListEntry);
                }

            }

            await _db.SaveChangesAsync();

            // Stop if MAL returned fewer than 500 items
            if (response.Data.Count < batchSize)
                break;

            offset += batchSize;
        }

        Console.WriteLine($"UserList import for '{statusType}' completed.");
    }
}
