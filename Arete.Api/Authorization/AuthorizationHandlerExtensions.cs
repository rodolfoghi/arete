using Microsoft.AspNetCore.Authorization;

namespace Arete.Api.Authorization;

public static class AuthorizationHandlerExtensions
{
    public static AuthorizationBuilder AddCurrentUserHandler(this AuthorizationBuilder builder)
    {
        builder.Services.AddScoped<IAuthorizationHandler, CheckCurrentUserAuthHandler>();
        return builder;
    }

    private class CheckCurrentUserRequirement : IAuthorizationRequirement
    {
    }

    private class CheckCurrentUserAuthHandler(CurrentUser currentUser)
        : AuthorizationHandler<CheckCurrentUserRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            CheckCurrentUserRequirement requirement)
        {
            if (currentUser.User is not null) context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}