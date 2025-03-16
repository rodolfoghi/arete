using Arete.Api;
using Arete.Api.Authorization;
using Arete.Api.Extensions;
using Arete.Api.Users;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDataProtection(o => o.ApplicationDiscriminator = "Arete.Api");

// Configure auth
builder.Services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
builder.Services.AddAuthorizationBuilder().AddCurrentUserHandler();

// Configure identity
builder.Services.AddIdentityCore<AreteUser>()
    .AddEntityFrameworkStores<AreteDbContext>()
    .AddApiEndpoints();

// Configure the database
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=.db/Arete.db";
builder.Services.AddSqlite<AreteDbContext>(connectionString);

// State that represents the current user from the database *and* the request
builder.Services.AddCurrentUser();

// Configure Open API
builder.Services.AddOpenApi(options => options.AddBearerTokenAuthentication());

// Configure rate limiting
builder.Services.AddRateLimiting();

builder.Services.AddHttpLogging(o =>
{
    if (builder.Environment.IsDevelopment())
    {
        o.CombineLogs = true;
        o.LoggingFields = HttpLoggingFields.ResponseBody | HttpLoggingFields.ResponseHeaders;
    }
});

var app = builder.Build();

app.UseHttpLogging();
app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.MapScalarApiReference(options =>
    {
        options.Servers = [];
        options.Authentication = new() { PreferredSecurityScheme = IdentityConstants.BearerScheme };
    });
}

app.MapOpenApi();

//app.MapDefaultEndpoints();

app.Map("/", () => Results.Redirect("/scalar/v1"));

// Configure the APIs
app.MapUsers();

app.Run();