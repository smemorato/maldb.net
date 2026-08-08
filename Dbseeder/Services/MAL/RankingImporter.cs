using Api.Mal;
using Api.Mal.Dtos.AnimeRanking;
using MyEfModels.Data;
using MyEfModels.Entities;
using Microsoft.EntityFrameworkCore;
using MyEfModels.Mapping;
using System.Text.Json;

namespace Dbseeder.Services;

public class RankingImporter
{
    private readonly IMalApiClient _mal;
    private readonly MyDbContext _db;
    private readonly RankingGenre _genreService;
    private readonly RankingStudio _studioService;
    public RankingImporter(IMalApiClient mal, MyDbContext db, RankingGenre genreService, RankingStudio studioService )
    {
        _mal = mal;
        _db = db;
        _genreService = genreService;
        _studioService = studioService;
        
    }

    public async Task ImportRankingAsync(string rankingType)
    {
        Console.WriteLine($"Importing ranking: {rankingType}");

        int offset = 0;
        const int batchSize = 500;

        while (true)
        {
            var response = await _mal.GetRankingAsync(rankingType, batchSize, offset);


            if (response == null)
            {
                Console.WriteLine($"❌ MAL returned NULL at offset {offset} (JSON parse failed)");
                break; // or continue; depending on your preference
            }

            if (response.Data == null)
            {
                Console.WriteLine($"❌ MAL returned NULL Data at offset {offset}");
                break;
            }

            if (response?.Data == null || response.Data.Count == 0)
                break;

            Console.WriteLine($"Fetched {response.Data.Count} items at offset {offset}");

            var items = response.Data;
            var genreLookup = await _genreService.LoadAndInsertGenresAsync(items);
            var studioLookup = await _studioService.LoadAndInsertStudiosAsync(items);


            // Process each anime in the batch
            foreach (var item in response.Data)
            {
                var id = item.Node.Id;

                // Load anime + rankings
                var anime = await _db.Animes
                    .Include(a => a.AnimeGenres)
                    .ThenInclude(ag => ag.Genre)
                    .Include(ag => ag.AnimeCompanies)
                    .ThenInclude(ag => ag.Company)
                    .FirstOrDefaultAsync(a => a.MalId == id);

                if (anime == null)
                {
                    // New anime discovered
                    anime = item.ToEntity();
                    try
                    {
                        _db.Animes.Add(anime);
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
                    item.UpdateEntity(anime);
                }

                //Sync genres (remove outdated + add missing)
                _genreService.SyncGenres(item, anime, genreLookup);
                _studioService.SyncStudios(item, anime, studioLookup);
            }

            await _db.SaveChangesAsync();

            // Stop if MAL returned fewer than 500 items
            if (response.Data.Count < batchSize)
                break;

            offset += batchSize;
        }

        Console.WriteLine($"Ranking import for '{rankingType}' completed.");
    }
}
