using MyEfModels.Entities;
using Api.Tenrai.Dtos.Anime;

namespace MyEfModels.Mapping.Tenrai;

public static class AnimeEpisodesMapping
{
    public static AnimeEpisode ToEntity(Anime anime, TenraiAnimeEpisodeDto animeEpisodeDto)
    {
        return new AnimeEpisode
        {
            Anime = anime,
            AnimeId = anime.Id,
            EpisodeNumber = animeEpisodeDto.Mal_Id,
            Url = animeEpisodeDto.Url,
            Title = animeEpisodeDto.Title,
            TitleRomanji = animeEpisodeDto.Title_Romanji,
            TitleJapanese = animeEpisodeDto.Title_Japanese,
            Duration = animeEpisodeDto.Duration,
            Aired = animeEpisodeDto.Aired,
            Score = animeEpisodeDto.Score,
            Filler = animeEpisodeDto.Filler,
            Recap = animeEpisodeDto.Recap,
            Synopsis = animeEpisodeDto.Synopsis,
            Replies = animeEpisodeDto.Replies,
            ForumUrl = animeEpisodeDto.Forum_Url,

        };
    }

    public static void UpdateEntity(
        AnimeEpisode entity,
        TenraiAnimeEpisodeDto animeEpisodeDto
        )
    {
            entity.Url = animeEpisodeDto.Url;
            entity.Title = animeEpisodeDto.Title;
            entity.TitleRomanji = animeEpisodeDto.Title_Romanji;
            entity.TitleJapanese = animeEpisodeDto.Title_Japanese;
            entity.Duration = animeEpisodeDto.Duration;
            entity.Aired = animeEpisodeDto.Aired;
            entity.Score = animeEpisodeDto.Score;
            entity.Filler = animeEpisodeDto.Filler;
            entity.Recap = animeEpisodeDto.Recap;
            entity.Synopsis = animeEpisodeDto.Synopsis;
            entity.Replies = animeEpisodeDto.Replies;
            entity.ForumUrl = animeEpisodeDto.Forum_Url;
    }
}


