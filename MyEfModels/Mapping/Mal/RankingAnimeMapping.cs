

using System.Security.Cryptography;
using Api.Mal.Dtos.AnimeRanking;
using MyEfModels.Entities;

namespace MyEfModels.Mapping;

public static class RankingAnimeMapping
{
    public static Anime ToEntity(this RankingItemDto dto)
    {
        var anime =  new Anime
        {
            MalId = dto.Node.Id,
            TitleRomanji = dto.Node.Title,
            TitleEnglish = dto.Node.Alternative_Titles?.En,
            TitleOriginal = dto.Node.Alternative_Titles?.Ja,
            TitleSynonyms = dto.Node.Alternative_Titles?.Synonyms ?? new List<string>(),
            MainPictureLarge = dto.Node.Main_Picture?.Large,
            MainPictureMedium = dto.Node.Main_Picture?.Medium,
            StartDate = dto.Node.Start_Date,
            EndDate = dto.Node.End_Date,
            Synopsis = dto.Node.Synopsis,
            MeanScore = dto.Node.Mean,
            Rank = dto.Node.Rank,
            Popularity = dto.Node.Popularity,
            ListUserCount = dto.Node.Num_List_Users,
            ScoringUserCount = dto.Node.Num_Scoring_Users,
            Nsfw = dto.Node.Nsfw,
            CreatedAt = dto.Node.Created_At,
            UpdatedAt = dto.Node.Updated_At,
            MediaType = dto.Node.Media_Type,
            Status = dto.Node.Status,
            NumberEpisodes = dto.Node.Num_Episodes,
            StartSeasonYear = dto.Node.Start_Season?.Year,
            StartSeasonSeason = dto.Node.Start_Season?.Season,
            BroadcastWeekday = dto.Node.Broadcast?.Day_Of_The_Week,
            BroadcastTime = dto.Node.Broadcast?.Start_Time,
            Source = dto.Node.Source,
            EpisodeDuration = dto.Node.Average_Episode_Duration,
            rating = dto.Node.Rating,
        };

        return anime;
    }
    public static IEnumerable<string> ExtractGenreNames(this RankingItemDto dto)
    {
        return dto.Node.Genres?.Select(g => g.Name)
               ?? Enumerable.Empty<string>();
    }

    public static IEnumerable<string> ExtractStudioNames(this RankingItemDto dto)
    {
        return dto.Node.Studios.Select(g => g.Name)
               ?? Enumerable.Empty<string>();
    }

    public static void UpdateEntity(this RankingItemDto dto, Anime anime)
    {
            anime.MalId = dto.Node.Id;
            anime.TitleRomanji = dto.Node.Title;
            anime.TitleEnglish = dto.Node.Alternative_Titles?.En;
            anime.TitleOriginal = dto.Node.Alternative_Titles?.Ja;
            anime.TitleSynonyms = dto.Node.Alternative_Titles?.Synonyms ?? new List<string>();
            anime.MainPictureLarge = dto.Node.Main_Picture?.Large;
            anime.MainPictureMedium = dto.Node.Main_Picture?.Medium;
            anime.StartDate = dto.Node.Start_Date;
            anime.EndDate = dto.Node.End_Date;
            anime.Synopsis = dto.Node.Synopsis;
            anime.MeanScore = dto.Node.Mean;
            anime.Rank = dto.Node.Rank;
            anime.Popularity = dto.Node.Popularity;
            anime.ListUserCount = dto.Node.Num_List_Users;
            anime.ScoringUserCount = dto.Node.Num_Scoring_Users;
            anime.Nsfw = dto.Node.Nsfw;
            anime.CreatedAt = dto.Node.Created_At;
            anime.UpdatedAt = dto.Node.Updated_At;
            anime.MediaType = dto.Node.Media_Type;
            anime.Status = dto.Node.Status;
            anime.NumberEpisodes = dto.Node.Num_Episodes;
            anime.StartSeasonYear = dto.Node.Start_Season?.Year;
            anime.StartSeasonSeason = dto.Node.Start_Season?.Season;
            anime.BroadcastWeekday = dto.Node.Broadcast?.Day_Of_The_Week;
            anime.BroadcastTime = dto.Node.Broadcast?.Start_Time;
            anime.Source = dto.Node.Source;
            anime.EpisodeDuration = dto.Node.Average_Episode_Duration;
            anime.rating = dto.Node.Rating;

    }
}
