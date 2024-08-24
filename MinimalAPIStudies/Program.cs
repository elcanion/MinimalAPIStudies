using Domain.Services;
using FluentValidation;
using Insfrastructure.SQL.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using MinimalAPIStudies.Endpoints;
using MinimalAPIStudies.Interfaces;
using MinimalAPIStudies.Mapping;
using MinimalAPIStudies.Mapping.Interfaces;
using MinimalAPIStudies.Models;
using MinimalAPIStudies.Routes;
using MinimalAPIStudies.Services;
using System.Net;
using System.Threading.RateLimiting;
using Microsoft.EntityFrameworkCore;
// Configure services
var builder = WebApplication.CreateBuilder(args);
var dbConnection = builder.Configuration.GetConnectionString("DemoDb");
builder.Services.AddRateLimiter(options => 
{
    options.RejectionStatusCode = (int)HttpStatusCode.TooManyRequests;
    options.OnRejected = async (context, token) =>
    {
        await context.HttpContext.Response.WriteAsync("Too many requests. Please try again later.");
    };
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext => RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress.ToString(),
            factory: _ => new FixedWindowRateLimiterOptions
            {
                QueueLimit = 10,
                PermitLimit = 50,
                Window = TimeSpan.FromSeconds(15)
            }));
});
builder.Services.AddScoped<IHelloService, HelloService>();
builder.Services.AddScoped<ICountryService, CountryService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();
builder.Services.AddScoped<ICountryMapper, CountryMapper>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
});
builder.Services.AddDbContextPool<DemoContext>(options => options.UseSqlServer(dbConnection));

// Configure and enable middlewares
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DemoContext>();
    db.Database.SetConnectionString(dbConnection);
    db.Database.Migrate();
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

app.AddCountryEndpoints();
app.UseRateLimiter();

app.Run();


