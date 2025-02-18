using GraphQLserver.Models;

namespace GraphQLserver.GraphQL
{
  public class CountryType : ObjectType<Country>
  {
    protected override void Configure(IObjectTypeDescriptor<Country> descriptor)
    {
      descriptor.Field(c => c.Venues)
          .ResolveWith<CountryResolvers>(r => r.GetVenues(default, default!, default))
          .UseFiltering()
          .UseSorting();
    }
  }
}