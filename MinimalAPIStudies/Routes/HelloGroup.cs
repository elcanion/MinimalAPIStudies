using Microsoft.AspNetCore.Mvc;
using MinimalAPIStudies.Interfaces;

namespace MinimalAPIStudies.Routes
{
    public static class HelloGroup
    {
        public static RouteGroupBuilder GroupHellos(this RouteGroupBuilder group)
        {
            group.MapGet("/{name}", ([FromRoute] string name, IHelloService service) => service.Hello(name));
            group.MapGet("/date/{date}", (DateTime date) => date.ToString());
            group.MapGet("/uniqueidentifier/{id}", (Guid id) => id.ToString());

            return group;
        }
    }
}
