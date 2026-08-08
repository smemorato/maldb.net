using Api.Tenrai;
using MyEfModels.Data;
using MyEfModels.Entities;
using Microsoft.EntityFrameworkCore;
using MyEfModels.Mapping.Tenrai;
using System.Text.Json;
using Api.Tenrai.Dtos.Anime;
using Dbseeder.Services.Shared;
using System.IO.Compression;

namespace Dbseeder.Services;

public class AnimeDetailsUpdater: AnimeUpdaterBase<TenraiAnimeDto>
{
    public AnimeDetailsUpdater(ITenraiApiClient tenrai, MyDbContext db)
        : base(tenrai, db) {}

    public override async Task<TenraiAnimeDto?> FetchDto(int id)
    {
        var response = await _tenrai.GetAnimeDetailsAsync(id);
        return response?.Data;
    }

    public override async Task ApplyUpdate(int id, TenraiAnimeDto dto)
    {
        var anime = await _db.Animes
            .FirstOrDefaultAsync(a => a.MalId == id);


        await _db.Entry(anime)
            .Collection(a => a.AnimeGenres)
            .Query()
            .Include(ag => ag.Genre)
            .LoadAsync();

        await _db.Entry(anime)
            .Collection(a => a.AnimeCompanies)
            .Query()
            .Include(ac => ac.Company)
            .LoadAsync();

        await _db.Entry(anime)
            .Collection(a => a.AnimeThemes)
            .Query()
            .LoadAsync();

        await _db.Entry(anime)
            .Collection(a => a.RelationsFrom)
            .LoadAsync();

        await _db.Entry(anime)
            .Collection(a => a.ExternalLinks)
            .LoadAsync();


        // Create anime if missing
        if (anime == null)
        {
            anime = TenraiAnimeDetailsMapping.ToEntity(dto);
            _db.Animes.Add(anime);
        }

        // Update core anime fields
        TenraiAnimeDetailsMapping.UpdateEntity(dto, anime);

        // Update genres
        UpdateGenres(dto, anime);

        // Update companies
        UpdateCompanies(dto.Studios, anime, "studio");
        UpdateCompanies(dto.Producers, anime, "producer");
        UpdateCompanies(dto.Licensors, anime, "licensor");

        // Update relations
        await InsertRelations(dto, anime);

        // Update themes (OP/ED)
        await InsertAnimeTheme(dto, anime);

        // Update external links
        try
        {
            await InsertAnimeExternalLinks(dto, anime);
        }
        catch
        {
            
        }

    }

    public async Task InsertRelations(TenraiAnimeDto item, Anime anime)
    {
        // Extract MAL IDs from DTO first (EF cannot translate this)
        var malIds = item.Relations
            .SelectMany(r => r.Entry)
            .Where(e => e.Type == "anime")
            .Select(e => e.Mal_id)
            .Distinct()
            .ToList();

        // Query DB only once
        var AnimeInDb = await _db.Animes
            .Where(a => malIds.Contains(a.MalId))
            .ToDictionaryAsync(a => a.MalId, a => a.Id);



        foreach (var relation in item.Relations)
        {
            foreach (var entry in relation.Entry)
            {
                if (entry.Type !=  "anime")
                {
                    continue;
                }

                if (!AnimeInDb.TryGetValue(entry.Mal_id, out var anime2Id))
                    continue;


                // Check if relation already exists
                var exists = anime.RelationsFrom
                    .Any(r => r.Type == relation.Relation &&
                            r.Anime2Id == anime2Id);

                if (exists)
                    continue;


                anime.RelationsFrom.Add(new AnimeRelation
                {
                    Type = relation.Relation,
                    Anime2Id = anime2Id
                });

                await _db.SaveChangesAsync();

                
            }
            
        }
    }

    public async  Task InsertAnimeExternalLinks(TenraiAnimeDto item, Anime anime)
    {
        var dtoLinks = item.External.ToDictionary(e => e.Url);

       foreach (var externalDto in  item.External)
        {
            var externalLink = anime.ExternalLinks
                .FirstOrDefault(el => el.Url == externalDto.Url);

            if  (externalLink == null)
            {
                externalLink = TenraiAnimeDetailsMapping.ToAnimeExternalLinkEntity(anime, externalDto);
                anime.ExternalLinks.Add(externalLink);
                _db.AnimeExternalLinks.Add(externalLink);
            }
        }

        // Remove missing
        var toRemove = anime.ExternalLinks
            .Where(el => !dtoLinks.ContainsKey(el.Url))
            .ToList();

        foreach (var rem in toRemove)
            _db.AnimeExternalLinks.Remove(rem);
        }

    public async  Task InsertAnimeTheme(TenraiAnimeDto item, Anime anime)
    {
        var animeOp = item.Theme.Openings ?? new List<string>();
        var animeEd = item.Theme.Endings ?? new List<string>();

        var opInDb = anime.AnimeThemes.Where( at => at.Type == "op" && at.Updated == false).Select(at => at.Title).ToHashSet();
        var edInDb = anime.AnimeThemes.Where( at => at.Type == "ed" && at.Updated == false).Select(at => at.Title).ToHashSet();


        var newOp = animeOp.Where(op => !opInDb.Contains(op));
        var newEd = animeEd.Where(ed => !edInDb.Contains(ed));


        
        var removedOp = opInDb.Where(op => !animeOp.Contains(op));
        var removedEd = edInDb.Where(ed => !animeEd.Contains(ed));

        // Remove openings
        foreach (var title in removedOp)
        {
            var theme = anime.AnimeThemes.First(at =>
                at.Type == "op" &&
                !at.Updated &&
                at.Title == title);

            anime.AnimeThemes.Remove(theme);
        }

        // Add openings
        foreach (var title in newOp)
        {
            anime.AnimeThemes.Add(new AnimeTheme
            {
                Title = title,
                Type = "op",
                Updated = false
            });
        }

        // Remove endings
        foreach (var title in removedEd)
        {
            var theme = anime.AnimeThemes.First(at =>
                at.Type == "ed" &&
                !at.Updated &&
                at.Title == title);

            anime.AnimeThemes.Remove(theme);
        }

        // Add endings
        foreach (var title in newEd)
        {
            anime.AnimeThemes.Add(new AnimeTheme
            {
                Title = title,
                Type = "ed",
                Updated = false
            });
        }


    }


    public void UpdateGenres(
        TenraiAnimeDto item,
        Anime anime)
    {
        var allGenreDtos = item.Genres
            .Concat(item.Explicit_Genres)
            .Concat(item.Themes)
            .Concat(item.Demographics)
            .ToList();

        foreach ( var genreDto in allGenreDtos)
        {
            var animeGenre = anime.AnimeGenres.Where(a => a.Genre?.Name == genreDto.Name).FirstOrDefault();

            if (animeGenre == null)
            {
                var genre = _db.Genres.Where(g => g.Name == genreDto.Name).FirstOrDefault();

                if (genre == null)
                {
                    genre = TenraiAnimeDetailsMapping.ToGenreEntity(genreDto);
                    _db.Genres.Add(genre);
                }

                animeGenre = TenraiAnimeDetailsMapping.ToAnimeGenreEntity(anime, genre);
                anime.AnimeGenres.Add(animeGenre);
                _db.AnimeGenres.Add(animeGenre);
            }
        }

        var dtoGenreIds = allGenreDtos
            .Select(g => g.Mal_id)
            .ToHashSet();

        var animeGenresToRemove = anime.AnimeGenres
            .Where(ag => !dtoGenreIds.Contains(ag.Genre.MalId));

        foreach ( var rem in animeGenresToRemove)
        {
            _db.AnimeGenres.Remove(rem);
        }


    }
        

    public void UpdateCompanies(
        List<TenraiAnimeCompanyDto> companyDtos,
        Anime anime,
        string role)
    {
            // Build a lookup of existing AnimeCompanies for this role
        var existingAnimeCompanies = anime.AnimeCompanies
            .Where(ac => ac.Role == role)
            .ToList();


        foreach ( var companyDto in companyDtos)
        {
            var animeCompany = existingAnimeCompanies
                .FirstOrDefault(ac => ac.Company.MalId == companyDto.Mal_Id);

            if (animeCompany == null)
            {
                var company = _db.Companies.Where(c => c.MalId == companyDto.Mal_Id).FirstOrDefault();

                if (company == null)
                {
                    company = TenraiAnimeDetailsMapping.ToCompanyEntity(companyDto);
                    _db.Companies.Add(company);
                }

                animeCompany = TenraiAnimeDetailsMapping.ToAnimeCompanyEntity(anime, company, role);
                anime.AnimeCompanies.Add(animeCompany);
                existingAnimeCompanies.Add(animeCompany);
                _db.AnimeCompanies.Add(animeCompany);
            }
        }

        var dtoCompanyIds = companyDtos
            .Select(g => g.Mal_Id)
            .ToHashSet();

        var toRemove = existingAnimeCompanies
            .Where(ac => !dtoCompanyIds.Contains(ac.Company.MalId))
            .ToList();

        foreach ( var rem in toRemove)
        {
            _db.AnimeCompanies.Remove(rem);
        }


    }
}
