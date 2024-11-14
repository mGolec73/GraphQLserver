using System.Linq;
using HotChocolate;
using HotChocolate.Data;
using GraphQLserver.Models;
using Microsoft.EntityFrameworkCore;


namespace GraphQLserver.GraphQL
{
    public class Queries
    {
        /// <summary>
        /// Retrieves a list of customers, including details about their country, ticket purchases, and associated venue.
        /// </summary>
        /// <param name="context">The database context used to retrieve customer data.</param>
        /// <returns>An IQueryable list of Customer objects with related country, ticket purchases, and venue data.</returns>
        [GraphQLDescription("Retrieves a list of countries, including their associated venues and customers.")]
        [UseFiltering]
        [UseSorting]

        public IQueryable<Country> GetCountries([Service] BMProjekt2024Context context) => context.Countries
            .Include(e => e.Venues)
            .Include(e => e.Customers);

        /// <summary>
        /// Retrieves a list of customers, including details about their country, ticket purchases, and associated venue.
        /// </summary>
        /// <param name="context">The database context used to retrieve customer data.</param>
        /// <returns>An IQueryable list of Customer objects with related country, ticket purchases, and venue data.</returns>
        [GraphQLDescription("Retrieves a list of customers, including details about their country, ticket purchases, and associated venue.")]
        [UseFiltering]
        [UseSorting]
        public IQueryable<Customer> GetCustomers([Service] BMProjekt2024Context context) => context.Customers
            .Include(e => e.CountryCodeNavigation)
            .Include(e => e.TicketPurchases)
            .Include(e => e.Venue);


        /// <summary>
        /// Retrieves a list of event sections with support for filtering and sorting, including details about the event, section, and associated tickets.
        /// </summary>
        /// <param name="context">The database context used to retrieve event section data.</param>
        /// <returns>An IQueryable list of EventSection objects with related event, section, and ticket data.</returns>
        [GraphQLDescription("Retrieves a list of event sections with support for filtering and sorting, including details about the event, section, and associated tickets.")]
        [UseFiltering]
        [UseSorting]
        public IQueryable<EventSection> GetEventSections([Service] BMProjekt2024Context context) => context.EventSections
            .Include(e => e.Event)
            .Include(e => e.Section)
            .Include(e => e.Tickets);

        /// <summary>
        /// Retrieves a list of events with support for filtering and sorting, including associated event sections and venue data.
        /// </summary>
        /// <param name="context">The database context used to retrieve event data.</param>
        /// <returns>An IQueryable list of Event objects with related event sections and venue data.</returns>
        [UseFiltering]
        [UseSorting]
        [GraphQLDescription("Retrieves a list of events with support for filtering and sorting, including associated event sections and venue data.")]
        public IQueryable<Event> GetEvents([Service] BMProjekt2024Context context) => context.Events
            .Include(e => e.EventSections)
            .Include(e => e.Venue);


        /// <summary>
        /// Retrieves a list of sections with support for filtering and sorting, including associated event sections and venue data.
        /// </summary>
        /// <param name="context">The database context used to retrieve section data.</param>
        /// <returns>An IQueryable list of Section objects with related event sections and venue data.</returns>
        [UseFiltering]
        [UseSorting]
        [GraphQLDescription("Retrieves a list of sections with support for filtering and sorting, including associated event sections and venue data.")]
        public IQueryable<Section> GetSections([Service] BMProjekt2024Context context) => context.Sections
            .Include(e => e.EventSections)
            .Include(e => e.Venue);


        /// <summary>
        /// Retrieves a list of tickets with support for filtering and sorting, including associated event sections and ticket purchase data.
        /// </summary>
        /// <param name="context">The database context used to retrieve ticket data.</param>
        /// <returns>An IQueryable list of Ticket objects with related event sections and ticket purchase data.</returns>
        [UseFiltering]
        [UseSorting]
        [GraphQLDescription("Retrieves a list of tickets with support for filtering and sorting, including associated event sections and ticket purchase data.")]
        public IQueryable<Ticket> GetTickets([Service] BMProjekt2024Context context) => context.Tickets
            .Include(e => e.EventSections)
            .Include(e => e.TicketPurchases);


        /// <summary>
        /// Retrieves a list of venue types with support for filtering and sorting, including associated venues.
        /// </summary>
        /// <param name="context">The database context used to retrieve venue type data.</param>
        /// <returns>An IQueryable list of VenueType objects with related venue data.</returns>
        [UseFiltering]
        [UseSorting]
        [GraphQLDescription("Retrieves a list of venue types with support for filtering and sorting, including associated venues.")]
        public IQueryable<VenueType_> GetVenueTypes([Service] BMProjekt2024Context context) => context.VenueTypes
            .Include(e => e.Venues);


        /// <summary>
        /// Retrieves a list of venues with support for filtering and sorting, including details about the country, associated customers, events, sections, and venue type.
        /// </summary>
        /// <param name="context">The database context used to retrieve venue data.</param>
        /// <returns>An IQueryable list of Venue objects with related country, customer, event, section, and venue type data.</returns>
        [UseFiltering]
        [UseSorting]
        [GraphQLDescription("Retrieves a list of venues with support for filtering and sorting, including details about the country, associated customers, events, sections, and venue type.")]
        public IQueryable<Venue> GetVenues([Service] BMProjekt2024Context context) => context.Venues
            .Include(e => e.CountryCodeNavigation)
            .Include(e => e.Customers)
            .Include(e => e.Events)
            .Include(e => e.Sections)
            .Include(e => e.VenueTypeNavigation); 
    }
}
