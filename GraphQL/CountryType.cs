using GraphQLserver.Models;
using GraphQLserver.Services;

namespace GraphQLserver.GraphQL
{
    public class CountryType : ObjectType<Country>
    {
        protected override void Configure(IObjectTypeDescriptor<Country> descriptor)
        {
            descriptor.Field(x => x.CountryCode);
            descriptor.Field(x => x.CountryName);
            descriptor.Field(x => x.Language);

            descriptor.Field(x => x.Customers)
                .ResolveWith<Resolvers>(r => r.GetCustomers(default!, default!))
                .UseProjection()
                .Name("customers");

            descriptor.Field(x => x.Venues)
                .ResolveWith<Resolvers>(r => r.GetVenues(default!, default!, default!))
                .UseProjection()
                .Name("venues");
        }
        private class Resolvers
        {
            
            public IQueryable<Customer> GetCustomers([Parent] Country country, [Service] BMProjekt2024Context dbContext)
            {
                return dbContext.Customers.Where(c => c.CountryCode == country.CountryCode);
            }

            public IQueryable<Venue> GetVenues([Parent] Country country, [Service] BMProjekt2024Context dbContext, [Service] ITenantIdResolverService tenantidresolver)
            {
                var tenantId = tenantidresolver.TenantId;
                return dbContext.Venues.Where(v => v.CountryCode == country.CountryCode && v.VenueId == tenantId);
            }
        }
    }

}
