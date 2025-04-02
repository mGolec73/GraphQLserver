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
        private static HashSet<string> GetRequestedFields(IResolverContext context)
        {
            var fields = new HashSet<string>();

            void ExtractFields(IEnumerable<HotChocolate.Language.ISelectionNode> selections, string parentPath = null)
            {
                foreach (var selection in selections)
                {
                    if (selection is HotChocolate.Language.FieldNode fieldNode)
                    {
                        var fieldPath = parentPath == null ? fieldNode.Name.Value : $"{parentPath}.{fieldNode.Name.Value}";
                        fields.Add(fieldPath);

                        if (fieldNode.SelectionSet?.Selections != null)
                        {
                            ExtractFields(fieldNode.SelectionSet.Selections, fieldPath);
                        }
                    }
                }
            }

            if (context.Selection.SyntaxNode.SelectionSet?.Selections != null)
            {
                ExtractFields(context.Selection.SyntaxNode.SelectionSet.Selections);
            }

            return fields;
        }

        /// <summary>
        /// retrieves a list of customers, including details about their country, ticket purchases, and associated venue.
        /// </summary>
        /// <param name = "context" > the database context used to retrieve customer data.</param>
        /// <returns>an iqueryable list of customer objects with related country, ticket purchases, and venue data.</returns>
        /// 
        [GraphQLDescription("Retrieves a list of countries, including their associated venues and customers.")]

        public async Task<List<Country>> GetCountriesAsync(
         [Service] BMProjekt2024Context dbContext,
         IResolverContext context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var selectedFields = GetRequestedFields(context);
            var tenantId = tenantIdResolver.TenantId;
            var countries = dbContext.Countries
                .Where(c => c.Venues.Any(v => v.VenueId == tenantId))
                .AsNoTracking();

            var fq = countries.SelectDynamic(selectedFields);
            return await fq.ToListAsync();
        }







        /// <summary>
        /// Retrieves a list of customers, including details about their country, ticket purchases, and associated venue.
        /// </summary>
        /// <param name="context">The database context used to retrieve customer data.</param>
        /// <returns>An IQueryable list of Customer objects with related country, ticket purchases, and venue data.</returns>
        [GraphQLDescription("Retrieves a list of customers, including details about their country, ticket purchases, and associated venue.")]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Customer> GetCustomers([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver) {
            var tenantId = tenantIdResolver.TenantId; 
            return context.Customers.Where(cu => cu.VenueId == tenantId); 
        }



        /// <summary>
        /// Retrieves a list of event sections with support for filtering and sorting, including details about the event, section, and associated tickets.
        /// </summary>
        /// <param name="context">The database context used to retrieve event section data.</param>
        /// <returns>An IQueryable list of EventSection objects with related event, section, and ticket data.</returns>
        [GraphQLDescription("Retrieves a list of event sections with support for filtering and sorting, including details about the event, section, and associated tickets.")]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<EventSection> GetEventSections([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            return context.EventSections.Where(es => es.VenueId == tenantId);
        }
        /// <summary>
        /// Retrieves a list of events with support for filtering and sorting, including associated event sections and venue data.
        /// </summary>
        /// <param name="context">The database context used to retrieve event data.</param>
        /// <returns>An IQueryable list of Event objects with related event sections and venue data.</returns>
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        [GraphQLDescription("Retrieves a list of events with support for filtering and sorting, including associated event sections and venue data.")]
        public IQueryable<Event> GetEvents([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            return context.Events.Where(e => e.VenueId == tenantId); ;
        }

        /// <summary>
        /// Retrieves a list of sections with support for filtering and sorting, including associated event sections and venue data.
        /// </summary>
        /// <param name="context">The database context used to retrieve section data.</param>
        /// <returns>An IQueryable list of Section objects with related event sections and venue data.</returns>
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        [GraphQLDescription("Retrieves a list of sections with support for filtering and sorting, including associated event sections and venue data.")]
        public IQueryable<Section> GetSections([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            return  context.Sections.Where(s => s.VenueId == tenantId);
        }

        /// <summary>
        /// Retrieves a list of tickets with support for filtering and sorting, including associated event sections and ticket purchase data.
        /// </summary>
        /// <param name="context">The database context used to retrieve ticket data.</param>
        /// <returns>An IQueryable list of Ticket objects with related event sections and ticket purchase data.</returns>
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        [GraphQLDescription("Retrieves a list of tickets with support for filtering and sorting, including associated event sections and ticket purchase data.")]
        public IQueryable<Ticket> GetTickets([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            return context.Tickets.Where(t => t.VenueId == tenantId);
        }

        /// <summary>
        /// Retrieves a list of venue types with support for filtering and sorting, including associated venues.
        /// </summary>
        /// <param name="context">The database context used to retrieve venue type data.</param>
        /// <returns>An IQueryable list of VenueType objects with related venue data.</returns>
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        [GraphQLDescription("Retrieves a list of venue types with support for filtering and sorting, including associated venues.")]
        public IQueryable<VenueType_> GetVenueTypes([Service] BMProjekt2024Context context) => context.VenueTypes;


        /// <summary>
        /// Retrieves a list of venues with support for filtering and sorting, including details about the country, associated customers, events, sections, and venue type.
        /// </summary>
        /// <param name="context">The database context used to retrieve venue data.</param>
        /// <returns>An IQueryable list of Venue objects with related country, customer, event, section, and venue type data.</returns>
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        [GraphQLDescription("Retrieves a list of venues with support for filtering and sorting, including details about the country, associated customers, events, sections, and venue type.")]
        public IQueryable<Venue> GetVenues([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            return context.Venues.Where(e => e.VenueId == tenantId);
        }
    }
}
