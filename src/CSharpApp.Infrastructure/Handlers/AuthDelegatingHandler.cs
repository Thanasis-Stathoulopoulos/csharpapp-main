using CSharpApp.Core.Interfaces;
using System.Net.Http.Headers;

namespace CSharpApp.Infrastructure.Handlers;

public class AuthDelegatingHandler(IAuthService authService) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await authService.GetTokenAsync();
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await base.SendAsync(request, cancellationToken);
    }
}