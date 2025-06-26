using GraphQLserver.GraphQL;
using GraphQLserver.Middleware;
using GraphQLserver.Models;
using GraphQLserver.Services;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with connection string from appsettings.json
builder.Services.AddDbContext<BMProjekt2024Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("baza")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        policy.SetIsOriginAllowed(origin => new Uri(origin).Host == "localhost")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddHttpContextAccessor(); 
builder.Services.AddScoped<ITenantIdResolverService, HeaderTenantIdResolverService>();


builder.Services
    .AddGraphQLServer()
    .RegisterDbContextFactory<BMProjekt2024Context>()
    .AddQueryType<Queries>()
    .UseField<GraphQLTenantMiddleware>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();
    

var app = builder.Build();

#region Needed for nginx and Kestrel (do not remove or change this region)
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
  ForwardedHeaders = ForwardedHeaders.XForwardedFor |
                     ForwardedHeaders.XForwardedProto
});
string? pathBase = app.Configuration["PathBase"];
if (!string.IsNullOrWhiteSpace(pathBase))
{
  app.UsePathBase(pathBase);
}
#endregion


app.UseCors("AllowLocalhost");

// Map GraphQL 
app.MapGraphQL();
app.Run();
