using HotChocolate.Resolvers;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using GraphQLserver.Services;
using GraphQLserver.Models;

namespace GraphQLserver.Middleware
{
    public class GraphQLTenantFilter
    {
        private readonly FieldDelegate _next;

        public GraphQLTenantFilter(FieldDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context, ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;

            // Provjeri je li rezultat queryable i dodaj filter
            if (context.Result is IQueryable<Venue> queryable)
            {
                context.Result = queryable.Where(v => v.VenueId == tenantId);
            }

            await _next(context);
        }
    }
}
