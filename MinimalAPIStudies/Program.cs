using Domain.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MinimalAPIStudies.Interfaces;
using MinimalAPIStudies.Mapping;
using MinimalAPIStudies.Mapping.Interfaces;
using MinimalAPIStudies.Models;
using MinimalAPIStudies.Routes;
using MinimalAPIStudies.Services;
// Configure services
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IHelloService, HelloService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<ICountryMapper, CountryMapper>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});

// Configure and enable middlewares
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));

app.MapGroup("/hello").GroupHellos();

app.MapPost("/Addresses", ([FromBody] Address address) =>
{
    return Results.Created();
});
app.MapPut("/Addresses/{addressId}", ([FromRoute] int addressId, [FromForm] Address address) =>
{
    return Results.NoContent();
}).DisableAntiforgery();
app.MapPost("/countries", ([FromBody] Country country, IValidator<Country> validator, ICountryMapper mapper) =>
{
    var validationResult = validator.Validate(country);
    if (validationResult.IsValid)
    {
        var countryDTO = mapper.Map(country);

        // Do some work here
        return Results.Created();
    }
    return Results.ValidationProblem(validationResult.ToDictionary());
});

// Create
app.MapPost("/countries", (
        [FromBody] Country country,
        IValidator<Country> ValidatorConfiguration,
        ICountryMapper mapper,
        ICountryService countryService) =>
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
                        }
                    );
            }
            return Results.ValidationProblem(
                validationResult.ToDictionary()
                );
        });

// Update
app.MapPut("/countries", (
    [FromBody] Country country,
    IValidator<Country> validator,
    ICountryMapper mapper,
    ICountryService countryService) =>
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
    });

// Delete
app.MapDelete("/countries/{id}", (
        int id,
        ICountryService countryService) => {
    if (countryService.Delete(id))
        return Results.NoContent();

    return Results.NotFound();
});

// Retrieve
app.MapGet("/countries/{id}", (
        int id, ICountryMapper mapper,
        ICountryService countryService) =>
{
    var country = countryService.Retrieve(id);

    if (country is null)
        return Results.NotFound();

    return Results.Ok(country);
}).WithName("countryById");

// Retrieve
app.MapGet("/countries", (
        ICountryMapper mapper,
        ICountryService countryService) =>
{
    var countries = countryService.GetAll();
    return Results.Ok(countries);
});

app.Run();


