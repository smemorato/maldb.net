namespace Api.Tenrai.Dtos.Anime;
public class TenraiRelationsDto
{
    public required string Relation { get; set; }
    public  required List<TenraiRelationMediaEntryDto> Entry { get; set; } = new List<TenraiRelationMediaEntryDto>();
}