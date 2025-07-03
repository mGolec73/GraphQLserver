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
        //[UseOffsetPaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Country> GetCountries([Service] BMProjekt2024Context dbContext)
        {

            return dbContext.Countries;
        }


       // [UseOffsetPaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Customer> GetCustomers([Service] BMProjekt2024Context dbContext) {

            return dbContext.Customers;
                
        }

        //[UseOffsetPaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<EventSection> GetEventSections([Service] BMProjekt2024Context dbContext, IResolverContext context)
        {

            return dbContext.EventSections;
        }
        //[UseOffsetPaging]
        [UseProjection]
        [UseFiltering]
        [UseSorting]  
        public IQueryable<Event> GetEvents([Service] BMProjekt2024Context dbContext, IResolverContext context)
        {
         
            return dbContext.Events; ;
        }
 
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Section> GetSections([Service] BMProjekt2024Context dbContext, IResolverContext context)
        {
            
            return dbContext.Sections;
        }
     
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Ticket> GetTickets([Service] BMProjekt2024Context dbContext, IResolverContext context)
        {

            return dbContext.Tickets;
        }

        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<VenueType> GetVenueTypes([Service] BMProjekt2024Context dbContext, IResolverContext context) {

        
            return dbContext.VenueTypes;
                
                }
        
        [UseProjection]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Venue> GetVenues([Service] BMProjekt2024Context dbContext, IResolverContext context)
        {
           
            return dbContext.Venues;
        }
    }
}
