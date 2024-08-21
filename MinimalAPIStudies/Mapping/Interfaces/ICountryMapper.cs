using Domain.DTOs;
using MinimalAPIStudies.Models;

namespace MinimalAPIStudies.Mapping.Interfaces
{
    public interface ICountryMapper
    {
        public CountryDTO? Map(Country country);
    }
}
