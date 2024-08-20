using Microsoft.AspNetCore.Mvc;
using MinimalAPIStudies.Interfaces;

namespace MinimalAPIStudies.Routes
{
    public static class HelloGroup
    {
        public static RouteGroupBuilder GroupHellos(this RouteGroupBuilder group)
        {

            var countries = new string[]
            {
                "France",
                "Canada",
                "USA"
            };

            var languages = new Dictionary<string, List<string>>()
            {
                { "France", new List<string> { "french" } },
                { "Canada", new List<string> { "french", "english"} },
                { "USA" , new List<string> { "english", "spanish" } }
            };

            group.MapGet("/", () => countries);
            group.MapGet("/{id}", (int id) => countries[id]);
            group.MapGet("/{id}/languages", (int id) =>
            {
                var country = countries[id];
                return languages[country];
            });
            return group;
        }
    }
}
