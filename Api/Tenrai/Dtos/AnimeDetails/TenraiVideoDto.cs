using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Runtime.CompilerServices;
using Api.Tenrai.Dtos.Anime;

namespace Api.Tenrai.Dtos.Anime;

public class TenraiVideoDto
{
    public string? Youtube_id { get; set; }
    public required string Url { get; set; }
    public string? Embed_Url {get; set; }
    public TenraiImageDto? Images { get; set; }
}