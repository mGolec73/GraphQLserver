using System.Linq;
using HotChocolate;
using HotChocolate.Data;
using GraphQLserver.Models;
using Microsoft.EntityFrameworkCore;
using GraphQLserver.Services;
using GraphQLserver.Middleware;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.EntityFrameworkCore.Internal;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using HotChocolate.Resolvers;
using GraphQL;
using HotChocolate.Execution.Processing;
using System.Linq.Dynamic.Core;
using Azure.Core;
using HotChocolate.Data.Projections.Context;


namespace GraphQLserver.GraphQL
{
  public class Queries
  {

    [GraphQLDescription("Retrieves a list of countries, including their associated venues and customers.")]
    [UseProjection]
    [UseFiltering]
    [UseSorting]    
    public IQueryable<Country> GetCountries([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
    {
      var tenantId = tenantIdResolver.TenantId;
      if (tenantId == null)
      {
        Console.WriteLine("TenantId not found");
      }
      else
      {
        Console.WriteLine($"TenantId found: {tenantId}");
      }
      return context.Countries;
    }


        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Customer> GetCustomers([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            return context.Customers.Where(cu => cu.VenueId == tenantId);
        }


        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<EventSection> GetEventSections([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            return context.EventSections.Where(es => es.VenueId == tenantId);
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Event> GetEvents([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            return context.Events.Where(e => e.VenueId == tenantId); ;
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Section> GetSections([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            return context.Sections.Where(s => s.VenueId == tenantId);
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Ticket> GetTickets([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            return context.Tickets.Where(t => t.VenueId == tenantId);
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<VenueType_> GetVenueTypes([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {

            var tenantId = tenantIdResolver.TenantId;
            return context.VenueTypes.Where(c => c.Venues.Any(v => v.VenueId == tenantId)); ;
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Venue> GetVenues([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            return context.Venues.Where(e => e.VenueId == tenantId);
        }
    }
      .Include(e => e.Events)
      .Include(e => e.Sections)
      .Include(e => e.VenueTypeNavigation)
      .Where(e => e.VenueId == tenantId);
    }
  }
}
