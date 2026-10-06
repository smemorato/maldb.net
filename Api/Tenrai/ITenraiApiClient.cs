
using Api.Tenrai.Dtos.Response;

namespace Api.Tenrai;

public interface ITenraiApiClient
{
    Task<TenraiAnimeResponseDto?> GetAnimeDetailsAsync(int id);
    Task<TenraiAnimeEpisodesResponseDto?> GetAnimeEpisodesAsync(int id, int page = 1);
    Task<TenraiAnimeRecommendationResponseDto?> GetAnimeRecommendationAsync(int id);
    Task<TenraiAnimeReviewsResponseDto?> GetAnimeReviewsAsync(int id, int page = 1);
    Task<TenraiAnimeCharactersResponseDto?> GetAnimeCharactersAsync (int id);
    Task<TenraiAnimeStaffResponseDto?> GetAnimeStaffAsync (int id);
    Task<TenraiAnimeStatisticsResponseDto?> GetAnimeStatisticsAsync (int id);
    Task<TenraiCharacterDetailsResponseDto?> GetCharacterDetails(int id);
    Task<TenraiPersonDetailsResponseDto?> GetPersonDetails(int id);
    Task<TenraiAnimeForumResponseDto?> GetAnimeForumTopics(int id, int page = 1);

}
