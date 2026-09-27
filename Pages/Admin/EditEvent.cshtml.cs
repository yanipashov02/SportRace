using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportRace.Domain.Entities;
using System.Net.Http.Json;

namespace SportRace.Web.Pages.Admin
{
    public class EditEventModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public Event Event { get; set; } = new();

        public EditEventModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var events = await client.GetFromJsonAsync<List<Event>>(
                "http://localhost:5012/api/events");

            Event = events?.FirstOrDefault(e => e.Id == id);

            if (Event == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var response = await client.PutAsJsonAsync(
                $"http://localhost:5012/api/events/{Event.Id}",
                Event);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Възникна грешка при записването на промените.");

                return Page();
            }

            return RedirectToPage("/Admin/Events");
        }
    }
}