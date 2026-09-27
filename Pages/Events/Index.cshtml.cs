using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using SportRace.Domain.Entities;

namespace SportRace.Web.Pages.Events
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<Event> Events { get; set; } = new();

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var events = await client.GetFromJsonAsync<List<Event>>("http://localhost:5012/api/events");

            if (events != null)
            {
                Events = events;
            }
        }
    }
}