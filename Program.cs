using Microsoft.EntityFrameworkCore;
using GraphQLserver.Models;
using HotChocolate.AspNetCore;
using GraphQLserver.GraphQL;
//using HotChocolate.AspNetCore.Playground;

var builder = WebApplication.CreateBuilder(args);

// Add DbContext with connection string from appsettings.json
builder.Services.AddDbContext<BMProjekt2024Context>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("baza")));

// Register GraphQL services
builder.Services
    .AddGraphQLServer()
    .RegisterDbContextFactory<BMProjekt2024Context>()
    .AddQueryType<Queries>()
    .AddProjections()
    .AddFiltering()
    .AddSorting();

var app = builder.Build();

// Map GraphQL and Playground endpoints
app.MapGraphQL();
//app.MapGraphQLPlayground(); // optional, for testing in the Playground UI

app.Run();
