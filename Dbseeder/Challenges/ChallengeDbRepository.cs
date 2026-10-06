using Microsoft.EntityFrameworkCore;
using MyEfModels.Data;
using MyEfModels.Entities;
using Api.Mal.Dtos.AnimeRanking;
using MyEfModels.Mapping;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json;

namespace Challenges;
public class AnimeChallengeRepository
{
    private readonly MyDbContext _db;
    private readonly HttpClient _httpClient;
    private const string MalApiUrl =
        "https://myanimelist.net/search/prefix.json";
    private int _userId;

    public AnimeChallengeRepository(MyDbContext db, HttpClient httpClient)
    {
        _db = db;
        _httpClient = httpClient;
    }

    public async Task<int> defineUser(string username)
    {
        _userId = await _db.Users
                .Where(u => u.Username == username)
                .Select(u => (int?)u.Id)
                .FirstOrDefaultAsync() 
                ?? throw new KeyNotFoundException($"User '{username}' was not found.");

            return _userId;
    }


    private async Task<int[]> FetchSearchIdsAsync(string title)
        {
            title = title.Trim();
            
            if (title.Length < 3)
                return Array.Empty<int>();

            string first3 = title[..3];
            string last3 = title[^3..];

            var idsFirst = await SearchMalAsync(first3);
            await Task.Delay(1000);
            var idsLast = await SearchMalAsync(last3);
            await Task.Delay(1000);

            return idsFirst.Concat(idsLast).Distinct().ToArray();
        }

        private async Task<int[]> SearchMalAsync(string keyword)
        {
            var parameters = new Dictionary<string, string>
            {
                { "type", "anime" },
                { "keyword", keyword },
                { "v", "1" }
            };

            try
            {
                var queryString = string.Join("&", parameters.Select(p => $"{p.Key}={Uri.EscapeDataString(p.Value)}"));
                var response = await _httpClient.GetAsync($"{MalApiUrl}?{queryString}");
                
                if (!response.Content.Headers.ContentType?.MediaType?.StartsWith("application/json") == true)
                {
                    Console.WriteLine($"[WARN] MAL returned non‑JSON for keyword '{keyword}'");
                    return Array.Empty<int>();
                }

                var content = await response.Content.ReadAsStringAsync();
                
                if (string.IsNullOrWhiteSpace(content))
                {
                    Console.WriteLine($"[WARN] MAL returned empty response for '{keyword}'");
                    return Array.Empty<int>();
                }

                using var jsonDoc = JsonDocument.Parse(content);
                var elements = jsonDoc.RootElement.GetProperty("categories")[0].GetProperty("items");
                
                var ids = new List<int>();
                foreach (var item in elements.EnumerateArray())
                {
                    if (item.TryGetProperty("id", out var idElement))
                        ids.Add(idElement.GetInt32());
                }

                return ids.ToArray();
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[ERROR] JSON decode failed for '{keyword}': {ex.Message}");
                return Array.Empty<int>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] MAL search failed for '{keyword}': {ex.Message}");
                return Array.Empty<int>();
            }
        }




    private async Task<IQueryable<UserList>>BaseFilter(int baseMalId,string formatFilter, string dateFrom )
    {

        var query = _db.UserList
            .Where(x => x.UserId == _userId
                && x.Status == "completed"
                && x.FinishDate != null
                && x.FinishDate.Length == 10  // Ensure valid format
                && x.FinishDate.CompareTo(dateFrom) >= 0  // Use CompareTo instead
                && x.Anime.MalId != baseMalId);

 
            // ✅ Apply media type filter based on mode
        if (formatFilter == "Movie" || formatFilter == "TV")
        {
                        // Exactly match the specified type
            query = query.Where(x => x.Anime.MediaType == formatFilter);

        }
        else
        {
            // Everything except TV and Movie
            query = query.Where(x => x.Anime.MediaType != "TV" 
                                && x.Anime.MediaType != "Movie");
        }

        return query;
    }

    // ==================== Duration Check ====================
    public async Task<List<int>> GetSameDuration(int baseMalId, string formatFilter, string dateFrom)
    {
        
        
        // Get base duration
        var baseDuration = await _db.Animes
            .Where(a => a.MalId == baseMalId)
            .Select(a => a.EpisodeDuration)
            .FirstOrDefaultAsync();

        
        if (!baseDuration.HasValue || baseDuration.Value <= 0)
            return new List<int>();

        var minuteThreshold = Math.Floor(baseDuration.Value / 60.0);

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);




        return await query
            .Where(x => x.Anime.EpisodeDuration.HasValue
                && Math.Floor(x.Anime.EpisodeDuration.Value / 60.0) == minuteThreshold)
            .OrderBy(x => x.FinishDate)
            .Select(x => x.Anime.MalId)
            .ToListAsync();
    }

    // ==================== Broadcast Day Check ====================
    public async Task<List<int>> GetSameBroadcast(int baseMalId, string formatFilter, string dateFrom)
    {
        
        // Helper function to get day of week
        string GetDayOfWeek(string? broadcastDay, string? startDate)
        {
            // Use the explicit broadcast day if available
            if (!string.IsNullOrWhiteSpace(broadcastDay))
            {
                return broadcastDay.Trim().ToLowerInvariant();
            }
                
            
            // Fall back to the start date
            if (DateTime.TryParse(startDate, out var dt))
            {
                return dt.DayOfWeek.ToString().ToLowerInvariant();
            }
                
            
            return string.Empty;
        }
        
        // Get base broadcast day
        var baseAnime = await _db.Animes
            .Where(a => a.MalId == baseMalId)
            .Select(a => new { a.BroadcastWeekday, a.StartDate })
            .FirstOrDefaultAsync();

        if (baseAnime == null)
        {
            return new List<int>();
        }


        var baseDay = GetDayOfWeek(baseAnime.BroadcastWeekday, baseAnime.StartDate);

        if (string.IsNullOrEmpty(baseDay))
        {
            return new List<int>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);




        var candidates = await query
            .Where(x =>
                (!string.IsNullOrWhiteSpace(x.Anime.BroadcastWeekday) &&
                x.Anime.BroadcastWeekday.Trim().ToLower() == baseDay)
                 || (string.IsNullOrWhiteSpace(x.Anime.BroadcastWeekday) &&
                     !string.IsNullOrWhiteSpace(x.Anime.StartDate) &&
                     x.Anime.StartDate.Length == 10)        
            )
            .OrderBy(x => x.FinishDate)
            .Select(x => new { x.Anime.MalId, x.Anime.BroadcastWeekday, x.Anime.StartDate })
            .ToListAsync();

        return candidates
            .Where(x =>
                !string.IsNullOrWhiteSpace(x.BroadcastWeekday)
                    ? x.BroadcastWeekday.Trim().ToLower() == baseDay
                    : DateTime.TryParse(x.StartDate, out var d)
                    && d.DayOfWeek.ToString().ToLower() == baseDay)
            .Select(x => x.MalId)
            .ToList();
    }

    // ==================== Starting Letter Check ====================
    public async Task<List<int>> GetSameStartingLetter(int baseMalId, string formatFilter, string dateFrom)
    {
        
        var baseLetter = await _db.Animes
            .Where(a => a.MalId == baseMalId)
            .Select(a => a.TitleRomanji.Substring(0, 1))
            .FirstOrDefaultAsync();


        if (string.IsNullOrEmpty(baseLetter))
        {
            return new List<int>();
        }


        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);


        return await query
            .Where(x => 
                x.Anime.TitleRomanji != null &&
                x.Anime.TitleRomanji.StartsWith(baseLetter)
            )
            .OrderBy(x => x.FinishDate)
            .Select(x => x.Anime.MalId)
            .ToListAsync();
    }

    // ==================== Search Bar Check (MAL IDs from API) ====================
    public async Task<List<int>> GetSearchBar(int baseMalId, string formatFilter, string dateFrom)
    {
        // TODO Update this so it doesn't need to query title again 
        var title = await _db.Animes
            .Where(a => a.MalId == baseMalId)
            .Select(a => a.TitleRomanji)
            .FirstOrDefaultAsync();
        
        if (string.IsNullOrEmpty(title))
        {
            return new List<int>();
        }

        var ids = await FetchSearchIdsAsync(title);

        if (ids.Length == 0)
        {
            return new List<int>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);

        return await query
            .Where(x => ids.Contains(x.Anime.MalId))
            .OrderBy(x => x.FinishDate)
            .Select(x => x.Anime.MalId)
            .ToListAsync();
    }

    // ==================== Recommendation Check ====================
    public async Task<List<int>> GetSameRecommendation(int baseMalId, string formatFilter, string dateFrom)
    {
        var recommendations = await _db.AnimeRecommendations
            .Where(r => r.Anime1.MalId == baseMalId)
            .Select(r => r.Anime2Id)
            .ToListAsync();
        
        if (recommendations == null || recommendations.Count == 0)
        {
            return new List<int>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);

        return await query
            .Where(x => recommendations.Contains(x.AnimeId))
            .OrderBy(x => x.FinishDate)
            .Select(x => x.Anime.MalId)
            .ToListAsync();
    }

    // ==================== Genre Check (Returns grouped dict) ====================
    public async Task<Dictionary<int, List<string>>> GetSameGenre(int baseMalId, string formatFilter, string dateFrom)
    {

        var baseGenres = await _db.AnimeGenres
            .Where(ag => ag.Anime.MalId == baseMalId)
            .Select(ag => ag.GenreId)
            .ToListAsync();


        if (baseGenres == null || baseGenres.Count == 0)
        {
            return new Dictionary<int, List<string>>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);

        var matches = await query
            .SelectMany(x => x.Anime.AnimeGenres
                .Where(ag => baseGenres.Contains(ag.GenreId))
                .Select(ag => new
                    {
                        Genre = ag.Genre.Name,
                        AnimeMalId = x.Anime.MalId,
                        x.FinishDate
                    }))
        .OrderBy(x => x.FinishDate)
        .ToListAsync();

        return matches
            .GroupBy(x => x.AnimeMalId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Genre)
                    .Distinct()
                    .ToList()
            );
    }

        // ==================== Scource Check ====================
    public async Task<List<int>> GetSameSource(int baseMalId, string formatFilter, string dateFrom)
    {
        
        var baseSource = await _db.Animes
            .Where(a => a.MalId == baseMalId)
            .Select(a => a.Source)
            .FirstOrDefaultAsync();

        if (baseSource == null)
        {
            return new List<int>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);

        return await query
            .Where(x => x.Anime.Source == baseSource)
            .OrderBy(x => x.FinishDate)
            .Select(x => x.Anime.MalId)
            .ToListAsync();
    }

    // ==================== Score Check ====================
    public async Task<List<int>> GetSameScore(int baseMalId, string formatFilter, string dateFrom)
    {
        
        var baseScore = await _db.Animes
            .Where(a => a.MalId == baseMalId)
            .Select(a => a.MeanScore)
            .FirstOrDefaultAsync();

        if (baseScore == null)
        {
            return new List<int>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);

        return await query
            .Where(x => x.Anime.MeanScore == baseScore)
            .OrderBy(x => x.FinishDate)
            .Select(x => x.Anime.MalId)
            .ToListAsync();
    }

    // ==================== Episodes Check ====================
    public async Task<List<int>> GetSameEpisodes(int baseMalId, string formatFilter, string dateFrom)
    {
        
        var baseEpisodes = await _db.Animes
            .Where(a => a.MalId == baseMalId)
            .Select(a => a.NumberEpisodes)
            .FirstOrDefaultAsync();

        if (baseEpisodes == null)
        {
            return new List<int>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);

        return await query
            .Where(x => x.Anime.NumberEpisodes == baseEpisodes)
            .OrderBy(x => x.FinishDate)
            .Select(x => x.Anime.MalId)
            .ToListAsync();
    }

    // ==================== Year/Month/Day Checks ====================
    public async Task<List<int>> GetSameYear(int baseMalId, string formatFilter, string dateFrom)
    {
        
        var baseYear = await _db.Animes
            .Where(a => a.MalId == baseMalId)
            .Select(a => a.StartDate != null && a.StartDate.Length >= 4
                ? a.StartDate.Substring(0, 4)
                : null)
        .FirstOrDefaultAsync();


        if (baseYear == null)
        {
            return new List<int>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);

        return await query
            .Where(x =>  x.Anime.StartDate != null
                    && x.Anime.StartDate.Length >= 4
                    && x.Anime.StartDate.Substring(0, 4) == baseYear)
            .OrderBy(x => x.FinishDate)
            .Select(x => x.Anime.MalId)
            .ToListAsync();
    }

    public async Task<List<int>> GetSameMonth(int baseMalId, string formatFilter, string dateFrom)
    {
        var BaseMonth = await _db.Animes
            .Where(a => a.MalId == baseMalId)
            .Select(a => a.StartDate != null && a.StartDate.Length >= 7
                ? a.StartDate.Substring(5, 2)
                : null)
        .FirstOrDefaultAsync();


        if (BaseMonth == null)
        {
            return new List<int>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);

        return await query
            .Where(x =>  x.Anime.StartDate != null
                    && x.Anime.StartDate.Length >= 7
                    && x.Anime.StartDate.Substring(5, 2) == BaseMonth)
            .OrderBy(x => x.FinishDate)
            .Select(x => x.Anime.MalId)
            .ToListAsync();
    }

    public async Task<List<int>> GetSameDay(int baseMalId, string formatFilter, string dateFrom)
    {
    
        var baseDay = await _db.Animes
            .Where(a => a.MalId == baseMalId)
            .Select(a => a.StartDate != null && a.StartDate.Length >= 10
                ? a.StartDate.Substring(8, 2)
                : null)
        .FirstOrDefaultAsync();


        if (baseDay == null)
        {
            return new List<int>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);

        return await query
            .Where(x =>  x.Anime.StartDate != null
                    && x.Anime.StartDate.Length >= 10
                    && x.Anime.StartDate.Substring(8, 2) == baseDay)
            .OrderBy(x => x.FinishDate)
            .Select(x => x.Anime.MalId)
            .ToListAsync();
    
    }



    // Creator check (creator, script, original concept)
    public async Task<Dictionary<int, List<int>>> GetSameCreator(int baseMalId, string formatFilter, string dateFrom)
    {

        var creatorRoles = new[] { "creator", "script", "original concept" };
        
        var basePersonIds = await _db.AnimeStaff
            .Where(x => x.Anime.MalId == baseMalId
                && creatorRoles.Any(role => x.Position.ToLower().Contains(role)))
            .Select(x => x.Person.MalId)
            .ToListAsync();

        if (basePersonIds == null || basePersonIds.Count == 0)
        {
            return new Dictionary<int, List<int>>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);


        var results = query
            .SelectMany(x => x.Anime.Staff
                    .Where(s => basePersonIds.Contains(s.Person.MalId))
                    .Select(s => new {
                        MalId =x.Anime.MalId,
                        PersonMalId = s.Person.MalId,
                        x.FinishDate
                        })
                    )
            .OrderBy(x => x.FinishDate)
            .GroupBy(x => x.PersonMalId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.MalId)
                    .Distinct()
                    .ToList()
            );

        return results;
    }

        public async Task<Dictionary<int, List<int>>> GetSameDirector(int baseMalId, string formatFilter, string dateFrom)
    {
        var directorRoles = new[] { "director", "storyboard" };
        
        var basePersonIds = await _db.AnimeStaff
            .Where(x => x.Anime.MalId == baseMalId
                && directorRoles.Any(role => x.Position.ToLower().Contains(role)))
            .Select(x => x.Person.MalId)
            .ToListAsync();

        if (basePersonIds == null || basePersonIds.Count == 0)
        {
            return new Dictionary<int, List<int>>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);


        var results = query
            .SelectMany(x => x.Anime.Staff
                    .Where(s => basePersonIds.Contains(s.Person.MalId))
                    .Select(s => new {
                        MalId =x.Anime.MalId,
                        PersonMalId = s.Person.MalId,
                        x.FinishDate
                        })
                    )
            .OrderBy(x => x.FinishDate)
            .GroupBy(x => x.PersonMalId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.MalId)
                    .Distinct()
                    .ToList()
            );

        return results;
    }

    public async Task<List<int>> GetSameStudio(int baseMalId, string formatFilter, string dateFrom)
    {
        string roleType = "studio";

        var baseStudioIds = await _db.AnimeCompanies
            .Where(x => x.Anime.MalId == baseMalId 
                && x.Company != null
                && x.Role.ToLower() == roleType)
            .Select(x => x.CompanyId)
            .ToListAsync();

        if (baseStudioIds == null || baseStudioIds.Count == 0)
        {
            return new List<int>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);


        return await query
            .Where(x => 
                    x.Anime.AnimeCompanies
                        .Any(ac => ac.Role.ToLower() == roleType 
                        && baseStudioIds.Contains(ac.CompanyId))
            )
            .OrderBy(x => x.FinishDate)
            .Select(x => x.Anime.MalId)
            .Distinct()
            .ToListAsync();

    }


    public async Task<List<int>> GetSameProducer(int baseMalId, string formatFilter, string dateFrom)
    {
        string roleType = "producer";

        var baseProducerIds = await _db.AnimeCompanies
            .Where(x => x.Anime.MalId == baseMalId 
                && x.Company != null
                && x.Role.ToLower() == roleType)
            .Select(x => x.CompanyId)
            .ToListAsync();

        if (baseProducerIds == null || baseProducerIds.Count == 0)
        {
            return new List<int>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);


        return await query
            .Where(x => 
                    x.Anime.AnimeCompanies
                        .Any(ac => ac.Role.ToLower() == roleType 
                        && baseProducerIds.Contains(ac.CompanyId))
            )
            .OrderBy(x => x.FinishDate)
            .Select(x => x.Anime.MalId)
            .Distinct()
            .ToListAsync();

    }


    public async Task<Dictionary<int, List<int>>> GetSameVA(int baseMalId, string formatFilter, string dateFrom)
    {

        var baseVAIds = await _db.AnimeCharacterVoiceActors
            .Where(x => x.AnimeCharacter.Anime.MalId == baseMalId && x.AnimeCharacter.Role == "Main")
            .Select(x => x.PersonId)
            .ToListAsync();

        if (baseVAIds == null || baseVAIds.Count == 0)
        {
            return new Dictionary<int, List<int>>();
        }

        var query = await BaseFilter(baseMalId, formatFilter, dateFrom);



        var results = query
            .SelectMany(x => x.Anime.AnimeCharacters
                    .Where(ac => ac.VoiceActors
                        .Any(va => baseVAIds.Contains(va.PersonId)))
                    .SelectMany(ac => ac.VoiceActors
                        .Where(va => baseVAIds.Contains(va.PersonId))
                        .Select(va => new
                        {
                            MalId = x.Anime.MalId,
                            PersonMalId = va.Person.MalId,
                            x.FinishDate
                        })
                    )
            )
            .OrderBy(x => x.FinishDate)
            .GroupBy(x => x.MalId)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.PersonMalId)
                    .Distinct()
                    .ToList()
            );

        return results;

    }
 
}