using GraphQLserver.Models;
using GraphQLserver.Services;

namespace GraphQLserver.GraphQL;

public class CountryResolvers
{
  public IQueryable<Venue> GetVenues([Parent] Country country, [Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
  {
    return context.Venues
                  .Where(v => v.VenueId == tenantIdResolver.TenantId); 
  }
}
