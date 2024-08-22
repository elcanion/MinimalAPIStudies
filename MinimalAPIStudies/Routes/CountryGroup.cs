using MinimalAPIStudies.Endpoints;

namespace MinimalAPIStudies.Routes
{
    public static class CountryGroup
    {
        public static void AddCountryEndpoints(this WebApplication app)
        {
            var group = app.MapGroup("/countries");

            group.MapPost("/", CountryEndpoints.PostCountry);
            group.MapPut("/", CountryEndpoints.PutCountry);
            group.MapDelete("/{id}", CountryEndpoints.DeleteCountry);
            group.MapGet("/{id}", CountryEndpoints.GetCountry).WithName("countryById");
            group.MapGet("/", CountryEndpoints.GetCountries);
        }
    }
}
