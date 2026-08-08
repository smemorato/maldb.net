
using System.Security.Cryptography;
using Api.Tenrai.Dtos.Anime;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyEfModels.Entities;

namespace MyEfModels.Mapping.Tenrai;

public static class TenraiAnimeDetailsMapping
{
    public static string TenraiDateToString(TenraiDateDto? dateDto)
    {
        if (dateDto is null || dateDto.year is null)
            return "";

        var result = $"{dateDto.year:D4}";

        if (dateDto.month is not null)
            result += $"-{dateDto.month:D2}";

        if (dateDto.day is not null)
            result += $"-{dateDto.day:D2}";

        return result;
    }
    public static Anime ToEntity(TenraiAnimeDto dto)
    {
        var anime = new Anime
        {
            MalId = dto.Mal_Id,
            TitleRomanji = dto.Title,
            TitleEnglish = dto.Title_English,
            TitleOriginal = dto.Title_Japanese,
            TitleSynonyms = dto.Title_Synonyms,
            Source = dto.Source,
            MediaType = dto.Type,
            NumberEpisodes = dto.Episodes,
            Status = dto.Status,
            StartDate = TenraiDateToString(dto.Aired?.Prop.From),
            EndDate = TenraiDateToString(dto.Aired?.Prop.To),
            MeanScore = dto.Score,
            ScoringUserCount = dto.Scored_By,
            Rank = dto.Rank,
            Popularity = dto.Popularity,
            ListUserCount = dto.Members,
            Favorites = dto.Favorites,

            Synopsis = dto.Synopsis,
            BackgroundInformation = dto.Background,
            StartSeasonYear = dto.Year,
            StartSeasonSeason = dto.Season,

            BroadcastWeekday = dto.BroadcastDto?.Day,
            BroadcastTime = dto.BroadcastDto?.Time,
            LastTenraiUpdate = DateOnly.FromDateTime(DateTime.UtcNow)


        };

        return anime;
    }


    public static AnimeGenre ToAnimeGenreEntity (Anime anime, Genre genre)
    {
        return new AnimeGenre
        {
          Anime = anime,
          AnimeId = anime.Id,
          Genre = genre,
          GenreId = genre.Id  
        };
    }

    public static AnimeCompany ToAnimeCompanyEntity (Anime anime, Company company, string role)
    {
        return new AnimeCompany
        {
          Anime = anime,
          AnimeId = anime.Id,
          Company = company,
          CompanyId = company.Id,  
          Role = role
        };
    }


    public static AnimeExternalLink ToAnimeExternalLinkEntity (Anime anime, TenraiExternalLinkDto dto)
    {
        return new AnimeExternalLink
        {
            Anime = anime,
            AnimeId = anime.Id,
            Name = dto.Name,
            Url = dto.Url
        };
    }

    public static Genre ToGenreEntity (TenraiGenreDto dto)
    {
        return new Genre
        {
            MalId = dto.Mal_id,
            Name = dto.Name,
            Type = dto.Type,
            Url = dto.Url
        };
    }

    public static Company ToCompanyEntity (TenraiAnimeCompanyDto dto)
    {
        return new Company
        {
            MalId = dto.Mal_Id,
            Name = dto.Name,
            Type = dto.Type,
            Url = dto.Url
        };
    }
    public static Dictionary<int,string> ExtractGenres(this TenraiAnimeDto dto)
    {
        if (dto is null)
            return new Dictionary<int, string>();

        return new[]
        {
            dto.Genres,
            dto.Explicit_Genres,
            dto.Themes,
            dto.Demographics
        }
        .Where(list => list != null)
        .SelectMany(list => list)
        .Where(c => c.Mal_id > 0 && !string.IsNullOrWhiteSpace(c.Name))
        .GroupBy(c => c.Mal_id) // avoid duplicates
        .ToDictionary(g => g.Key, g => g.First().Name);
    }
 



    public static void UpdateEntity(TenraiAnimeDto dto, Anime anime)
    {
            anime.TitleRomanji = dto.Title;
            anime.TitleEnglish = dto.Title_English;
            anime.TitleOriginal = dto.Title_Japanese;
            anime.TitleSynonyms = dto.Title_Synonyms;
            anime.Source = dto.Source;
            anime.MediaType = dto.Type;
            anime.NumberEpisodes = dto.Episodes;
            anime.Status = dto.Status;
            anime.StartDate = TenraiDateToString(dto.Aired?.Prop.From);
            anime.EndDate = TenraiDateToString(dto.Aired?.Prop.To);
            anime.MeanScore = dto.Score;
            anime.ScoringUserCount = dto.Scored_By;
            anime.Rank = dto.Rank;
            anime.Popularity = dto.Popularity;
            anime.ListUserCount = dto.Members;
            anime.Favorites = dto.Favorites;
            anime.Synopsis = dto.Synopsis;
            anime.BackgroundInformation = dto.Background;
            anime.StartSeasonYear = dto.Year;
            anime.StartSeasonSeason = dto.Season;
            anime.BroadcastWeekday = dto.BroadcastDto?.Day;
            anime.BroadcastTime = dto.BroadcastDto?.Time;
            anime.LastTenraiUpdate = DateOnly.FromDateTime(DateTime.UtcNow);
    }
}
