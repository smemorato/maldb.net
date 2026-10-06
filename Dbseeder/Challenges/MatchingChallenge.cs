using Microsoft.EntityFrameworkCore;
using MyEfModels.Data;
using MyEfModels.Entities;
using Api.Mal.Dtos.AnimeRanking;
using MyEfModels.Mapping;
using System.Text.Json;



namespace Challenges;
public class MatchingChallenge
{
    private readonly MyDbContext _db;
    private readonly AnimeChallengeRepository _repo;
    public MatchingChallenge(
    MyDbContext db,
    AnimeChallengeRepository repo)
    {
    _db = db;
    _repo = repo;
    }

    public async Task RunMatchingChallenge(string type, DateOnly startDate, string username, int matchMinimumForSearch)
    {
        int userId = await _repo.defineUser(username);


        var query =  _db.UserList
            .Where(ul => ul.User.Username == username 
                && ul.Status == "completed");

                
        
                    // ✅ Apply media type filter based on mode
        if (type == "Movie" || type == "TV")
        {
                        // Exactly match the specified type
            query = query.Where(x => x.Anime.MediaType == type);

        }
        else
        {
            // Everything except TV and Movie
            query = query.Where(x => x.Anime.MediaType != "TV" 
                                && x.Anime.MediaType != "Movie");
        }

        var baseItems = await query.Select(ul => new
                {
                    ul.Anime.MalId,
                    ul.FinishDate,
                    ul.Anime.TitleRomanji
                }).ToListAsync();


        
        var checks = new Dictionary<string, Func<int, string, string, Task<object?>>>
            {
                { "duration", async (id, m, d) => await _repo.GetSameDuration(id, m, d) },
                { "broadcast", async (id, m, d) => await _repo.GetSameBroadcast(id, m, d) },
                { "starting_letter", async (id, m, d) => await _repo.GetSameStartingLetter(id, m, d) },
                { "recommendation", async (id, m, d) => await _repo.GetSameRecommendation(id, m, d) },
                { "genre", async (id, m, d) => await _repo.GetSameGenre(id, m, d) },
                { "score", async (id, m, d) => await _repo.GetSameScore(id, m, d) },
                { "source", async (id, m, d) => await _repo.GetSameSource(id, m, d) },
                { "episodes", async (id, m, d) => await _repo.GetSameEpisodes(id, m, d) },
                { "year", async (id, m, d) => await _repo.GetSameYear(id, m, d) },
                { "month",async (id, m, d) => await _repo.GetSameMonth(id, m, d) },
                { "day", async(id, m, d) => await _repo.GetSameDay(id, m, d) },
                { "creator", async (id, m, d) => await _repo.GetSameCreator(id, m, d) },
                { "director", async (id, m, d) => await _repo.GetSameDirector(id, m, d) },
                { "studio", async (id, m, d) => await _repo.GetSameStudio(id, m, d) },
                { "VA", async (id, m, d) => await _repo.GetSameVA(id, m, d) },
                { "producer", async (id, m, d) => await _repo.GetSameProducer(id, m, d) },
                { "search_bar", async (id, m, d) => await _repo.GetSearchBar(id,m, d) }
            };
        
        var options = new Dictionary<int, Dictionary<string, object>>();
        int counter = 0;

        foreach (var item in baseItems)
        {
            var startDateString = startDate.ToString("yyyy-MM-dd");
            var dateToUse = !string.IsNullOrEmpty(item.FinishDate) && string.Compare(item.FinishDate, startDateString)>0
                ? item.FinishDate 
                : startDateString;

            

            var passed = new Dictionary<string, object>();
            var failed = new List<string>();
            int matchcounter = 0;

            foreach (var kvp in checks)
            {
                string key = kvp.Key;
                Func<int, string, string, Task<object?>> func = kvp.Value;

                object? value = null;

                if (key != "search_bar" || (key == "search_bar" && matchcounter >= matchMinimumForSearch) )
                {
                    value = await func(item.MalId, type, dateToUse);
                }
                

            // Fail if value is null or an empty collection/list
                if (value is null || 
                    (value is System.Collections.ICollection collection && collection.Count == 0) ||
                    (value is System.Collections.IEnumerable enumerable && !enumerable.Cast<object>().Any()))
                {
                    
                    failed.Add(key);
                    continue;
                    
                }

                matchcounter++;


                passed[key] = value!;
            }

            int score = passed.Count;
            options[item.MalId] = new Dictionary<string, object>
            {
                { "score", score },
                { "passed", passed },
                { "failed", failed }
            };
            counter ++;

        }

        // Sort by score descending
        var sortedOptions = options.OrderByDescending(x => x.Value["score"])
                                    .ToDictionary(x => x.Key, x => x.Value);

        // Write to CSV
        string[] checkColumns = new[]
        {
            "duration", "broadcast", "starting_letter", 
            "recommendation", "genre", "score", "source", "episodes",
            "year", "month", "day", "creator", "director", "studio", "VA", "producer", "search_bar"
        };

        using var writer = new StreamWriter("sorted_options.csv", false, System.Text.Encoding.UTF8);
        writer.WriteLine("mal_id,score," + string.Join(",", checkColumns));

        foreach (var kvp in sortedOptions)
        {
            int malId = kvp.Key;
            var data = kvp.Value;

            var row = new List<string> { malId.ToString(), data["score"].ToString() };

            foreach (var column in checkColumns)
            {
                

                if (data.ContainsKey("passed") && 
                    data["passed"] is Dictionary<string, object> passed &&
                    passed.ContainsKey(column))
                {
                    var value = passed[column];

                    string formattedValue = value switch
                    {
                    // Bypass static type checks using dynamic so C# reads actual runtime generic types
                        System.Collections.IEnumerable or System.Collections.IDictionary => 
                            JsonSerializer.Serialize((dynamic)value),

                        _ => value?.ToString() ?? string.Empty
                    };

                    // Escape CSV values containing commas or quotes
                    if (formattedValue.Contains(",") || formattedValue.Contains("\"") || formattedValue.Contains("\n"))
                    {
                        formattedValue = $"\"{formattedValue.Replace("\"", "\"\"")}\"";
                    }

                    row.Add(formattedValue);
                }
                else
                {
                    row.Add(string.Empty);
                }
            }

            await writer.WriteLineAsync(string.Join(",", row));
        }

        Console.WriteLine();
    }

}