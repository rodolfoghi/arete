using Arete.Api.Users;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Arete.Api;

public class AreteDbContext(DbContextOptions<AreteDbContext> options) : IdentityDbContext<AreteUser>(options)
{
}