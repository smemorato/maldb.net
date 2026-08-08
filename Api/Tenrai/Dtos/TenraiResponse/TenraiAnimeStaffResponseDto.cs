using Api.Tenrai.Dtos.Anime;

namespace Api.Tenrai.Dtos.Response;

public class TenraiAnimeStaffResponseDto
{
    public List<TenraiAnimeStaffDto> Data { get; set; } = new List<TenraiAnimeStaffDto>();
}
