using System.Net.Http;
using System.Text.Json;
using Api.Mal.Dtos.AnimeDetails;
using Api.Mal.Dtos.UserAnimeList;
using Api.Mal.Dtos.AnimeRanking;
using Api.Mal.Dtos;
using Microsoft.Extensions.Options;
using System.ComponentModel;


namespace Api.Mal;

public class MalApiClient : IMalApiClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions;

    public MalApiClient(HttpClient httpClient, IOptions<MalApiOptions> options)
    {
        _http = httpClient;
        _http.BaseAddress = new Uri("https://api.myanimelist.net/v2/");
        _http.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", options.Value.ClientId);

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
            WriteIndented = false
        };
    }
    private static void PrintErrorValue(string json, JsonException ex)
    {
        long bytePos = ex.BytePositionInLine ?? 0;

        long pos = Math.Min(bytePos, int.MaxValue); // safe cast
        long start = Math.Max(0, pos - 200);
        long length = Math.Min(400, json.Length - start);

        string snippet = json.Substring((int)start, (int)length);

        Console.WriteLine("❌ JSON ERROR SNIPPET:");
        Console.WriteLine(snippet);

    }



    private async Task<T?> GetAsync<T>(string url)
    {
        var response = await _http.GetAsync(url);
        if (!response.IsSuccessStatusCode)
            return default;

        var json = await response.Content.ReadAsStringAsync();
        var jsonElement = JsonSerializer.Deserialize<JsonElement>(json);
        var jsonDocument = JsonSerializer.Deserialize<JsonDocument>(json);
        var dict = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
        var abc = JsonSerializer.Deserialize<object>(json);
        string pretty = JsonSerializer.Serialize(
            jsonElement,
            new JsonSerializerOptions { WriteIndented = true }
        );


        try
        {
            var bob = JsonSerializer.Deserialize<T>(json, _jsonOptions);
            return bob;    
            //return await JsonSerializer.DeserializeAsync<T>(stream, _jsonOptions);
        }
        catch (JsonException ex)
        {
            Console.WriteLine(pretty);
            Console.WriteLine("JSON error:");
            Console.WriteLine(ex.Message);
            Console.WriteLine($"Path: {ex.Path}");
            Console.WriteLine($"Line: {ex.LineNumber}, Byte: {ex.BytePositionInLine}");
            PrintErrorValue(json, ex);
            return default;
        }


    }

    // -----------------------------
    // Anime Details
    // -----------------------------
    public Task<AnimeDto?> GetAnimeDetailsAsync(int id, string fields = "")
    {
        fields = "id,title,main_picture,alternative_titles,start_date,end_date,synopsis,mean,rank,popularity,num_list_users,num_scoring_users,nsfw,created_at,updated_at,media_type,status,genres,my_list_status,num_episodes,start_season,broadcast,source,average_episode_duration,rating,pictures,background,related_anime,related_manga,recommendations,studios,statistics";
        string url = $"anime/{id}?fields={fields}";
        return GetAsync<AnimeDto>(url);
    }

    // -----------------------------
    // User Anime List
    // -----------------------------
    public Task<UserAnimeListDto?> GetUserAnimeListAsync(
        string username, int limit = 100, int offset = 0,  string status = "", string sort = "")
    {
        string fields = "list_status{tags}";
        
        string url = $"users/{username}/animelist?limit={limit}&offset={offset}&fields={fields}&status={status}&sort={sort}";
        return GetAsync<UserAnimeListDto>(url);
    }

    // -----------------------------
    // Ranking
    // -----------------------------
    public Task<RankingResponseDto?> GetRankingAsync(string rankingType = "all", int limit = 500, int offset = 0, string fields = "")
    {

        fields = "id,title,main_picture,alternative_titles,start_date,end_date,synopsis,mean,rank,popularity,num_list_users,num_scoring_users,nsfw,created_at,updated_at,media_type,status,genres,my_list_status,num_episodes,start_season,broadcast,source,average_episode_duration,rating,pictures,background,related_anime,related_manga,recommendations,studios,statistics";
        
        string url = $"anime/ranking?ranking_type={rankingType}&limit={limit}&offset={offset}&fields={fields}";
        return GetAsync<RankingResponseDto>(url);
    }



    public Task<UserDto?> GetUserAsync(string username)
    {

        string url = $"users/{username}";
        return GetAsync<UserDto>(url);
    }

}