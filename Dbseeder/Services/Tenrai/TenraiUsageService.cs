using Microsoft.EntityFrameworkCore;
using MyEfModels.Data;
using MyEfModels.Entities;


public class TenraiUsageService
{
    private readonly MyDbContext _db;
    private const int DailyLimit = 40000;

    public TenraiUsageService(MyDbContext db)
    {
        _db = db;
    }

    public async Task<int> IncrementAsync()
    {
        var today = DateTime.UtcNow.Date;

        var counter = await _db.TenraiUsageCounters.FirstOrDefaultAsync(c => c.Date == today);
       
        if (counter == null)
        {
            counter = new TenraiUsageCounter
            {
                Date = today,
                Count = 0
            };

            _db.TenraiUsageCounters.Add(counter);
        }

        counter.Count++;

        if (counter.Count > DailyLimit)
        {
            throw new InvalidOperationException(
                $"Tenrai daily limit exceeded ({DailyLimit} requests).");
        }

        await _db.SaveChangesAsync();

        return counter.Count;
    }
}
