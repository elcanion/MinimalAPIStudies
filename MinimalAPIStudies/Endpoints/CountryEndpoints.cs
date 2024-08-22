using Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using MinimalAPIStudies.Mapping.Interfaces;
using MinimalAPIStudies.Models;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;

namespace MinimalAPIStudies.Endpoints
{
    public static class CountryEndpoints
    {
        public static IResult PostCountry(
            [FromBody] Country country,
            IValidator<Country> ValidatorConfiguration,
            ICountryMapper mapper,
            ICountryService countryService)
        {
            var validationResult = ValidatorConfiguration.Validate(country);
            if (validationResult.IsValid)
            {
                var countryDTO = mapper.Map(country);
                return Results.CreatedAtRoute(
                        "countryById",
                        new
                        {
                            Id = countryService.CreateOrUpdate(countryDTO)
                        });
            }
        return Results.ValidationProblem(validationResult.ToDictionary());
        }

        public static IResult PutCountry(
            [FromBody] Country country,
            IValidator<Country> validator,
            ICountryMapper mapper,
            ICountryService countryService)
        {
        var validationResult = validator.Validate(country);

        if (validationResult.IsValid)
        {
            if (country.Id is null)
            {
                return Results.CreatedAtRoute(
                    "countryById",
                    new
                    {
                        Id = countryService.CreateOrUpdate(mapper.Map(country))
                    });
            }
            return Results.NoContent();
            }
            return Results.ValidationProblem(
                validationResult.ToDictionary()
            );
        }

        public static IResult DeleteCountry(
            int id,
            ICountryService countryService)
        {
            if (countryService.Delete(id))
                return Results.NoContent();

            return Results.NotFound();
        }

        public static IResult GetCountry(
            int id, 
            ICountryMapper mapper,
            ICountryService countryService)
        {
            var country = countryService.Retrieve(id);

            if (country is null)
                return Results.NotFound();

            return Results.Ok(country);
        }

        public static IResult GetCountries(
            ICountryMapper mapper,
            ICountryService countryService)
        {
            var countries = countryService.GetAll();
            return Results.Ok(countries);
        }
    }
}
