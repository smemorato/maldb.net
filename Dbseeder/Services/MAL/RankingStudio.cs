using Microsoft.EntityFrameworkCore;
using MyEfModels.Data;
using MyEfModels.Entities;
using Api.Mal.Dtos.AnimeRanking;
using MyEfModels.Mapping;


namespace Dbseeder.Services;

public class RankingStudio
{
    private readonly MyDbContext _db;

    public RankingStudio(MyDbContext db)
    {
        _db = db;
        
    }

    public async Task<Dictionary<string, Company>> LoadAndInsertStudiosAsync(
        IEnumerable<RankingItemDto> items)
    {

        // get all Studios from ranking batch
        var allStudioNames = items
            .SelectMany(item => item.ExtractStudioNames())
            .ToHashSet();

        if (allStudioNames.Count == 0)
            return new Dictionary<string, Company>();


        // compare Studios in batch with existing Studios
        var existingStudios = await _db.Companies
            .Where(s => allStudioNames.Contains(s.Name))
            .ToListAsync();

        var existingStudioNames = existingStudios
            .Select(g => g.Name)
            .ToHashSet();

        var newStudios = allStudioNames
            .Where(name => !existingStudioNames.Contains(name))
            .Select(name => new Company { Name = name })
            .ToList();

        // add new Studios
        if (newStudios.Count > 0)
        {
            _db.Companies.AddRange(newStudios);
            await _db.SaveChangesAsync();
        }

        return existingStudios
            .Concat(newStudios)
            .ToDictionary(s => s.Name, s => s);
    }

    public void SyncStudios(
        RankingItemDto item,
        Anime anime,
        Dictionary<string, Company> studioLookup)
    {

        var malStudioNames = item.ExtractStudioNames().ToHashSet();

        // Remove outdated relations
        var studiosToRemove = anime.AnimeCompanies
            .Where(astd => astd.Company != null && !malStudioNames.Contains(astd.Company.Name) && astd.Role == "studio")
            .ToList();

        foreach (var studio in studiosToRemove)
            anime.AnimeCompanies.Remove(studio);

        // Add missing relations
        foreach (var name in malStudioNames)
        {
            if (!studioLookup.TryGetValue(name, out var studio))
                continue;

            bool exists = anime.AnimeCompanies.Any(astd => astd.CompanyId == studio.Id);

            if (!exists)
            {
                anime.AnimeCompanies.Add(new AnimeCompany
                {
                    Anime = anime,
                    Company = studio,
                    Role = "studio"
                });
            }
        }
    }

}
