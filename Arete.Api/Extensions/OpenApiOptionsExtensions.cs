using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace Arete.Api.Extensions;

public static class OpenApiOptionsExtensions
{
    public static OpenApiOptions AddBearerTokenAuthentication(this OpenApiOptions options)
    {
        var scheme = new OpenApiSecurityScheme
        {
            Type = SecuritySchemeType.Http,
            Name = IdentityConstants.BearerScheme,
            Scheme = "Bearer",
            Reference = new OpenApiReference
            {
                Type = ReferenceType.SecurityScheme,
                Id = IdentityConstants.BearerScheme
            }
        };
        options.AddDocumentTransformer((document, _, _) =>
        {
            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes.Add(IdentityConstants.BearerScheme, scheme);
            return Task.CompletedTask;
        });
        options.AddOperationTransformer((operation, context, _) =>
        {
            if (context.Description.ActionDescriptor.EndpointMetadata.OfType<IAuthorizeData>().Any())
                operation.Security = [new OpenApiSecurityRequirement { [scheme] = [] }];

            return Task.CompletedTask;
        });

        return options;
    }
}