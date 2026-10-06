using MyEfModels.Entities;
using Api.Tenrai.Dtos.Anime;

namespace MyEfModels.Mapping.Tenrai;

public static class AnimeReviewsMapping
{
    public static AnimeReview ToEntity(Anime anime, TenraiAnimeReviewDto animeReviewDto)
    {
        return new AnimeReview
        {
            Anime = anime,
            AnimeId = anime.Id,
            MalId = animeReviewDto.Mal_Id,
            Type = animeReviewDto.Type,
            Date = animeReviewDto.Date,
            Review = animeReviewDto.Review,
            Score = animeReviewDto.Score,
            Tags = animeReviewDto.Tags,
            Overall = animeReviewDto.Reactions?.Overall,
            Nice = animeReviewDto.Reactions?.Nice,
            LoveIt = animeReviewDto.Reactions?.Love_It,
            Funny = animeReviewDto.Reactions?.Funny,
            Confusing = animeReviewDto.Reactions?.Confusing,
            Informative = animeReviewDto.Reactions?.Informative,
            WellWritten = animeReviewDto.Reactions?.Well_Written,
            Creative = animeReviewDto.Reactions?.Creative
        };
    }

    public static void UpdateEntity(
        AnimeReview entity,
        TenraiAnimeReviewDto animeReviewDto
        )
    {
            entity.Review = animeReviewDto.Review;
            entity.Score = animeReviewDto.Score;
            entity.Tags = animeReviewDto.Tags;
            entity.Overall = animeReviewDto.Reactions?.Overall;
            entity.Nice = animeReviewDto.Reactions?.Nice;
            entity.LoveIt = animeReviewDto.Reactions?.Love_It;
            entity.Funny = animeReviewDto.Reactions?.Funny;
            entity.Confusing = animeReviewDto.Reactions?.Confusing;
            entity.Informative = animeReviewDto.Reactions?.Informative;
            entity.WellWritten = animeReviewDto.Reactions?.Well_Written;
            entity.Creative = animeReviewDto.Reactions?.Creative;
    }
}


