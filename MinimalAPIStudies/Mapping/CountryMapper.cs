using Domain.DTOs;
using MinimalAPIStudies.Mapping.Interfaces;
using MinimalAPIStudies.Models;

namespace MinimalAPIStudies.Mapping
{
    public class CountryMapper : ICountryMapper
    {
        public CountryDTO? Map(Country country)
        {
            return country is not null ? new CountryDTO
            {
                Name = country.Name,
                Description = country.Description,
                FlagUri = country.FlagUri,
            } : null;
        }
    }
}
