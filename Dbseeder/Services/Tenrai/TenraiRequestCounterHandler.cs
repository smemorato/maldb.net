namespace Dbseeder.Services;


public class TenraiRequestCounterHandler : DelegatingHandler
{
    private readonly TenraiUsageService _usageService;

    public TenraiRequestCounterHandler(TenraiUsageService usageService)
    {
        _usageService = usageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        await _usageService.IncrementAsync();
        return await base.SendAsync(request, cancellationToken);
    }
}
