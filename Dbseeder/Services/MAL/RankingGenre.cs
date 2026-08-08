using Microsoft.EntityFrameworkCore;
using MyEfModels.Data;
using MyEfModels.Entities;
using Api.Mal.Dtos.AnimeRanking;
using MyEfModels.Mapping;


namespace Dbseeder.Services;

public class RankingGenre
{
    private readonly MyDbContext _db;

    public RankingGenre(MyDbContext db)
    {
        _db = db;
        
    }

    public async Task<Dictionary<string, Genre>> LoadAndInsertGenresAsync(
        IEnumerable<RankingItemDto> items)
    {

        // get all genre from ranking batch
        var allGenreNames = items
            .SelectMany(item => item.ExtractGenreNames())
            .ToHashSet();

        if (allGenreNames.Count == 0)
            return new Dictionary<string, Genre>();


        // compare genres in batch with existing genres
        var existingGenres = await _db.Genres
            .Where(g => allGenreNames.Contains(g.Name))
            .ToListAsync();

        var existingGenreNames = existingGenres
            .Select(g => g.Name)
            .ToHashSet();

        var newGenres = allGenreNames
            .Where(name => !existingGenreNames.Contains(name))
            .Select(name => new Genre { Name = name })
            .ToList();

        // add new genres
        if (newGenres.Count > 0)
        {
            _db.Genres.AddRange(newGenres);
            await _db.SaveChangesAsync();
        }

        return existingGenres
            .Concat(newGenres)
            .ToDictionary(g => g.Name, g => g);
    }

    public void SyncGenres(
        RankingItemDto item,
        Anime anime,
        Dictionary<string, Genre> genreLookup)
    {

        var malGenreNames = item.ExtractGenreNames().ToHashSet();

        // Remove outdated relations
        var genresToRemove = anime.AnimeGenres
            .Where(ag => ag.Genre != null && !malGenreNames.Contains(ag.Genre.Name))
            .ToList();

        foreach (var ag in genresToRemove)
            anime.AnimeGenres.Remove(ag);

        // Add missing relations
        foreach (var name in malGenreNames)
        {
            if (!genreLookup.TryGetValue(name, out var genre))
                continue;

            bool exists = anime.AnimeGenres.Any(ag => ag.GenreId == genre.Id);

            if (!exists)
            {
                anime.AnimeGenres.Add(new AnimeGenre
                {
                    Anime = anime,
                    Genre = genre
                });
            }
        }
    }

}
