namespace Dbseeder.Api.Mal;

public class MalAuthHandler : DelegatingHandler
{
    private readonly string _clientId;

    public MalAuthHandler(string clientId)
    {
        _clientId = clientId;
    }

    protected override Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        request.Headers.Add("X-MAL-CLIENT-ID", _clientId);
        return base.SendAsync(request, cancellationToken);
    }
}
