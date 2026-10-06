// ITenraiApiClient is your existing interface
using Api.Tenrai;
using MyEfModels.Data;
using MyEfModels.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dbseeder.Services.Shared;

public class AnimeIdResolver
{
    private readonly MyDbContext _db;

    public AnimeIdResolver(MyDbContext db)
    {
        _db = db;
    }

    public async Task<List<int>> ResolveFromUserList(
        string username,
        string? startDate = null,
        string? status = null)
    {
        var userId = await _db.Users
            .Where(u => u.Username == username)
            .Select(u => (int?)u.Id)
            .FirstOrDefaultAsync();

        if (userId == null)
        {
            Console.WriteLine($"User with Username {username} is not in the db");
            return new List<int>();
        }

        var query = _db.UserList
            .Where(ul => ul.UserId == userId);

        if (!string.IsNullOrEmpty(startDate))
        {
            query = query.Where(ul => ul.StartDate != null &&
                                      ul.StartDate.CompareTo(startDate) >= 0);
        }

        if (!string.IsNullOrEmpty(status))
        {
            query = query.Where(ul => ul.Status == status);
        }

        return await query
            .Select(ul => ul.Anime!.MalId)
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<int>> ResolveFromAnimeTable(
        string? username = null,
        string? startDate = null,
        string? status = null,
        bool onlyNew = true,
        DateOnly? lastUpdate = null,
        int? startFrom = null)
    {
        var query = _db.Animes.AsQueryable();

        if (username != null)
        {
            query = query.Where(a => a.UserLists.Any(ul => ul.User.Username == username));
        }

        if (startDate != null)
        {
            query = query.Where(a => a.UserLists.Any(ul => ul.StartDate.CompareTo(startDate) >= 0));
        }

        if (status != null)
        {
            query = query.Where(a => a.UserLists.Any(ul => ul.Status == status));
        }

        if (lastUpdate != null)
        {
            query = query.Where(a => a.LastTenraiUpdate < lastUpdate );
        }
        if (startFrom != null)
        {
            query = query.Where(a => a.MalId >= startFrom );
        }

        return await query
            .Select(a => a.MalId)
            .Distinct()
            .OrderBy(id => id)
            .ToListAsync();
    }


        public async Task<List<int>> ResolveFromCharacterTable(
        string? username = null,
        string? startDate = null,
        string? status = null,
        bool onlyNew = true,
        DateOnly? lastUpdate = null,
        int? startFromAnimeMalId = null)
    {
        var query = _db.Animes
                    .Include(a => a.AnimeCharacters)
                    .ThenInclude(ac => ac.Character)
                    .AsQueryable();

        if (username != null)
        {
            query = query.Where(a => a.UserLists.Any(ul => ul.User.Username == username));
        }

        if (startDate != null)
        {
            query = query.Where(a => a.UserLists.Any(ul => ul.StartDate.CompareTo(startDate) >= 0));
        }

        if (status != null)
        {
            query = query.Where(a => a.UserLists.Any(ul => ul.Status == status));
        }

        if (lastUpdate != null)
        {
            query = query.Where(a => a.LastTenraiUpdate < lastUpdate );
        }
        if (startFromAnimeMalId != null)
        {
            query = query.Where(a => a.MalId >= startFromAnimeMalId );
        }

        return await query
            .SelectMany(a => a.AnimeCharacters)
                .Select(ac => ac.Character.MalId)
            .Distinct()
            .OrderBy(id => id)
            .ToListAsync();
    }

    
}
