using Api.Mal.Dtos.AnimeDetails;
using Api.Mal.Dtos.UserAnimeList;
using Api.Mal.Dtos.AnimeRanking;
using Api.Mal.Dtos;
namespace Api.Mal;

public interface IMalApiClient
{
    Task<AnimeDto?> GetAnimeDetailsAsync(int id, string fields = "");
    Task<UserAnimeListDto?> GetUserAnimeListAsync(
        string username, int limit = 100, int offset = 0,  string status = "", string sort = "");
    
    Task<RankingResponseDto?> GetRankingAsync(string rankingType = "all", int limit = 10, int offset = 0, string fields = "");
    Task<UserDto?>  GetUserAsync(string username);
}
