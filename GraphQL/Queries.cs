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



        [UseProjection]
        public IQueryable<Country> GetCountries(
         [Service] BMProjekt2024Context dbContext,
         IResolverContext context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            var countries = dbContext.Countries
                .Where(c => c.Venues.Any(v => v.VenueId == tenantId));
            return  countries;
        }

        
        
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Customer> GetCustomers([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver) {
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
            return  context.Sections.Where(s => s.VenueId == tenantId);
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
        public IQueryable<VenueType_> GetVenueTypes([Service] BMProjekt2024Context context) => context.VenueTypes;
        
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Venue> GetVenues([Service] BMProjekt2024Context context, [Service] ITenantIdResolverService tenantIdResolver)
        {
            var tenantId = tenantIdResolver.TenantId;
            return context.Venues.Where(e => e.VenueId == tenantId);
        }
    }
}
