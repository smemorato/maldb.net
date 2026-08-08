using System.Net.Http;
using System.Text.Json;
using Microsoft.Extensions.Options;
using System.ComponentModel;
using Api.Tenrai.Dtos.Response;


namespace Api.Tenrai;

public class TenraiApiClient : ITenraiApiClient
{
    private readonly HttpClient _http;
    private readonly JsonSerializerOptions _jsonOptions;

    public TenraiApiClient(HttpClient httpClient)
    {
        _http = httpClient;
        _http.BaseAddress = new Uri("https://api.tenrai.org/v1/");
        //_http.DefaultRequestHeaders.Add("X-MAL-CLIENT-ID", options.Value.ClientId);

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
        var json = await response.Content.ReadAsStringAsync();


        var pretty = JsonSerializer.Serialize(
            JsonSerializer.Deserialize<JsonElement>(json),
            new JsonSerializerOptions { WriteIndented = true }
        );

        int status = (int)response.StatusCode;

        if (status >= 200 && status < 300)
        {
            // 204 No Content → return null safely
            if (status == 204 || string.IsNullOrWhiteSpace(json))
            {
                Console.WriteLine($"ℹ️ {status} No Content for {url}");
                return default;
            }

            try
            {
                var result = JsonSerializer.Deserialize<T>(json, _jsonOptions);
                if (result == null)
                    Console.WriteLine($"❌ Deserialized NULL\n{pretty}");

                return result;
            }
            catch (JsonException ex)
            {
                Console.WriteLine("❌ JSON parse error:");
                Console.WriteLine(pretty);
                Console.WriteLine(ex.Message);
                Console.WriteLine($"Path: {ex.Path}");
                Console.WriteLine($"Line: {ex.LineNumber}, Byte: {ex.BytePositionInLine}");
                PrintErrorValue(json, ex);
                return default;
            }
        }

        // -----------------------------
        // CLIENT ERRORS
        // -----------------------------
        if (status == 404)
        {
            Console.WriteLine($"❌ 404 Not Found: {url}");
            return default;
        }

        if (status == 429)
        {
            Console.WriteLine("⏳ 429 Too Many Requests — retrying in 2 seconds");
            await Task.Delay(2000);
            return await GetAsync<T>(url);
        }

        // -----------------------------
        // SERVER ERRORS
        // -----------------------------
        if (status == 503)
        {
            Console.WriteLine("⚠️ 503 Service Unavailable — retrying in 3 seconds");
            await Task.Delay(3000);
            return await GetAsync<T>(url);
        }

        if (status == 500)
        {
            Console.WriteLine("💥 500 Internal Server Error");
            Console.WriteLine(pretty);
            return default;
        }

        // -----------------------------
        // OTHER ERRORS
        // -----------------------------
        Console.WriteLine($"❌ HTTP {status} Error");
        Console.WriteLine(pretty);
        return default;
    }



    public Task<TenraiAnimeResponseDto?> GetAnimeDetailsAsync(int id)
    {
        string url = $"anime/{id}/full";
        return GetAsync<TenraiAnimeResponseDto>(url);
    }
    public Task<TenraiAnimeRecommendationResponseDto?> GetAnimeRecommendationAsync(int id)
    {
        string url = $"anime/{id}/recommendations";
        return GetAsync<TenraiAnimeRecommendationResponseDto>(url);
    }
    public Task<TenraiAnimeCharactersResponseDto?> GetAnimeCharactersAsync (int id)
    {
        string url = $"anime/{id}/characters";
        return GetAsync<TenraiAnimeCharactersResponseDto>(url);
    }
    public Task<TenraiAnimeStaffResponseDto?> GetAnimeStaffAsync (int id)
    {
        string url = $"anime/{id}/staff";
        return GetAsync<TenraiAnimeStaffResponseDto>(url);
    }
    public Task<TenraiAnimeStatisticsResponseDto?> GetAnimeStatisticsAsync (int id)
    {
        string url = $"anime/{id}/statistics";
        return GetAsync<TenraiAnimeStatisticsResponseDto>(url);
    }

    public Task<TenraiCharacterDetailsResponseDto?> GetCharacterDetails(int id)
    {
        string url = $"characters/{id}/full";
        return GetAsync<TenraiCharacterDetailsResponseDto>(url);
    }

    public Task<TenraiPersonDetailsResponseDto?> GetPersonDetails(int id)
    {
        string url = $"people/{id}/full";
        return GetAsync<TenraiPersonDetailsResponseDto>(url);
    }


    public Task<TenraiAnimeForumResponseDto?> GetAnimeForumTopics(int id, int page = 1)
    {
        string url = $"anime/{id}/forum?page={page}";
        return GetAsync<TenraiAnimeForumResponseDto>(url);
    }


}