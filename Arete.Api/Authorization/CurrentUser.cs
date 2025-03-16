using System.Security.Claims;
using Arete.Api.Users;

namespace Arete.Api.Authorization;

public class CurrentUser
{
    public AreteUser? User { get; set; }
    public ClaimsPrincipal Principal { get; set; } = default!;
}