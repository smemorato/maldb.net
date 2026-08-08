namespace Api.Tenrai.Dtos.Anime;

public class TenraiPersonEntryDto
{
    public int Mal_Id { get; set; }
    public required string Url { get; set; }
    public required string Name { get; set; }
    //some Endpoints don't return favories for example anime voice actore return favorites be animestaff doesn't
    public int Favorites { get; set; }
    public TenraiImagesDto?  Images { get; set; }
}