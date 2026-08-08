namespace Api.Tenrai.Dtos.Anime;

public class TenraiAnimeForumTopicDto
{
    public int Mal_id { get; set; }
    public required string Url { get; set; }
    public required string Title { get; set; }
    public required string Date { get; set; }
    public required string Author_Username { get; set; }
    public required string Author_Url { get; set; }
    public int Comments { get; set;}
    public required TenraiAnimeForumTopicLastComment Last_Comment { get; set;}
}