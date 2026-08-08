using Api.Tenrai.Dtos.Anime;

namespace Api.Tenrai.Dtos.Response;

public class TenraiAnimeCharactersResponseDto
{
    public List<TenraiAnimeCharacterDto> Data { get; set; } = new List<TenraiAnimeCharacterDto>();
}
