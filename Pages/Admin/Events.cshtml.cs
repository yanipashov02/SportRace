using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportRace.Domain.Entities;
using System.Net.Http.Json;

namespace SportRace.Web.Pages.Admin
{
    public class EventsModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<Event> Events { get; set; } = new();

        public EventsModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var events = await client.GetFromJsonAsync<List<Event>>(
                "http://localhost:5012/api/events");

            if (events != null)
            {
                Events = events;
            }
        }
        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var response = await client.DeleteAsync(
                $"http://localhost:5012/api/events/{id}");

            return RedirectToPage();
        }
    }
}