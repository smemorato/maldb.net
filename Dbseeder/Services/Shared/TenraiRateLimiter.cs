namespace Dbseeder.Services.Shared;

public class TenraiRateLimiter
{
    private readonly int _intervalMs;
    private DateTime _nextRequestTime;

    public TenraiRateLimiter(int intervalMs = 1000)
    {
        _intervalMs = intervalMs;
        _nextRequestTime = DateTime.UtcNow;
    }

    public async Task WaitAsync()
    {
        var now = DateTime.UtcNow;
        if (now < _nextRequestTime)
        {
            var delay = _nextRequestTime - now;
            await Task.Delay(delay);
        }

        _nextRequestTime = DateTime.UtcNow.AddMilliseconds(_intervalMs);
    }
}
