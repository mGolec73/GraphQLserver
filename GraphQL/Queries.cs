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


    [GraphQLDescription("Retrieves a list of customers, including details about their country, ticket purchases, and associated venue.")]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Customer> GetCustomers([Service] BMProjekt2024Context context) => context.Customers
        .Include(e => e.CountryCodeNavigation)
        .Include(e => e.TicketPurchases)
        .Include(e => e.Venue);



    [GraphQLDescription("Retrieves a list of event sections with support for filtering and sorting, including details about the event, section, and associated tickets.")]
    [UseFiltering]
    [UseSorting]
    public IQueryable<EventSection> GetEventSections([Service] BMProjekt2024Context context) => context.EventSections
        .Include(e => e.Event)
        .Include(e => e.Section)
        .Include(e => e.Tickets);

   
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieves a list of events with support for filtering and sorting, including associated event sections and venue data.")]
    public IQueryable<Event> GetEvents([Service] BMProjekt2024Context context) => context.Events
        .Include(e => e.EventSections)
        .Include(e => e.Venue);


  
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieves a list of sections with support for filtering and sorting, including associated event sections and venue data.")]
    public IQueryable<Section> GetSections([Service] BMProjekt2024Context context) => context.Sections
        .Include(e => e.EventSections)
        .Include(e => e.Venue);


    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieves a list of tickets with support for filtering and sorting, including associated event sections and ticket purchase data.")]
    public IQueryable<Ticket> GetTickets([Service] BMProjekt2024Context context) => context.Tickets
        .Include(e => e.EventSections)
        .Include(e => e.TicketPurchases);


    
    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieves a list of venue types with support for filtering and sorting, including associated venues.")]
    public IQueryable<VenueType_> GetVenueTypes([Service] BMProjekt2024Context context) => context.VenueTypes
        .Include(e => e.Venues);


    [UseFiltering]
    [UseSorting]
    [GraphQLDescription("Retrieves a list of venues with support for filtering and sorting, including details about the country, associated customers, events, sections, and venue type.")]
    public IQueryable<Venue> GetVenues([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
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

      return context.Venues
      .Include(e => e.CountryCodeNavigation)
      .Include(e => e.Customers)
      .Include(e => e.Events)
      .Include(e => e.Sections)
      .Include(e => e.VenueTypeNavigation)
      .Where(e => e.VenueId == tenantId);
    }
  }
}
