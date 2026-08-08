using MyEfModels.Entities;
using Api.Tenrai.Dtos.Anime;

namespace MyEfModels.Mapping.Tenrai;

public static class AnimeRecommendationMapping
{
    public static AnimeRecommendation ToEntity(
        TenraiAnimeRecommendationDto animeRecommendationDto,
        Anime anime,
        Anime recommendedAnime)
    {
        return new AnimeRecommendation
        {
            Anime1Id = anime.Id,
            Anime2Id = recommendedAnime.Id,
            Votes = animeRecommendationDto.Votes
        };
    }

    public static void UpdateEntity(
        TenraiAnimeRecommendationDto animeRecommendationDto,
        AnimeRecommendation entity)
    {
        entity.Votes = animeRecommendationDto.Votes;
    }
}
