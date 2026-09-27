using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportRace.Domain.Entities;
using System.Net.Http.Json;

namespace SportRace.Web.Pages.Events
{
    public class DetailsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public Event? Event { get; set; }

        public DetailsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var events = await client.GetFromJsonAsync<List<Event>>("api/events");

            Event = events?.FirstOrDefault(e => e.Id == id);
        }
    }
}