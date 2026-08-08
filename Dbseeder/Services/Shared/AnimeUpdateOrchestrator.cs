using Dbseeder.Services.Shared;
using MyEfModels.Data;

namespace Dbseeder.Services;

public class AnimeUpdateOrchestrator
{
    private readonly MyDbContext _db;
    private readonly TenraiRateLimiter _rateLimiter;

    public AnimeUpdateOrchestrator(MyDbContext db, TenraiRateLimiter rateLimiter)
    {
        _db = db;
        _rateLimiter = rateLimiter;
    }

    /// <summary>
    /// Runs an update pipeline for a list of anime IDs using a specific updater.
    /// </summary>
    public async Task RunAsync<TDto>(
        List<int> ids,
        IAnimeUpdater<TDto> updater)
    {
        foreach (var id in ids)
        {
            // Respect Tenrai API rate limit
            await _rateLimiter.WaitAsync();

            // Fetch DTO from Tenrai
            var dto = await updater.FetchDto(id);
            if (dto == null)
            {
                Console.WriteLine($"❌ Tenrai returned NULL for id {id}");
                continue;
            }

            // Apply EF update logic
            await updater.ApplyUpdate(id, dto);

            // Save changes after each anime update
            await _db.SaveChangesAsync();
        }
    }
}
