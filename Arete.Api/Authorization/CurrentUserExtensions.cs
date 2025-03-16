using System.Security.Claims;
using Arete.Api.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;

namespace Arete.Api.Authorization;

public static class CurrentUserExtensions
{
    public static IServiceCollection AddCurrentUser(this IServiceCollection services)
    {
        services.AddScoped<CurrentUser>();
        services.AddScoped<IClaimsTransformation, ClaimsTransformation>();
        return services;
    }

    private sealed class ClaimsTransformation(CurrentUser currentUser, UserManager<AreteUser> userManager)
        : IClaimsTransformation
    {
        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            currentUser.Principal = principal;
            if (principal.FindFirstValue(ClaimTypes.NameIdentifier) is { Length: > 0 } id)
                currentUser.User = await userManager.FindByIdAsync(id);

            return principal;
        }
    }
}