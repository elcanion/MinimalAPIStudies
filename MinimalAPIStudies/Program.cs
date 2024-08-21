using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MinimalAPIStudies.Interfaces;
using MinimalAPIStudies.Mapping;
using MinimalAPIStudies.Mapping.Interfaces;
using MinimalAPIStudies.Models;
using MinimalAPIStudies.Routes;
using MinimalAPIStudies.Services;
using System.Net;
// Configure services
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IHelloService, HelloService>();
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

app.Run();


