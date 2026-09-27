using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportRace.Domain.Entities;
using System.Net.Http.Json;

namespace SportRace.Web.Pages.Admin.Participants
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public Participant Participant { get; set; } = new();

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var participants =
                await client.GetFromJsonAsync<List<Participant>>(
                    "api/participants"
                );

            var participant = participants?
                .FirstOrDefault(p => p.Id == id);

            if (participant == null)
            {
                return NotFound();
            }

            Participant = participant;

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
                $"api/participants/{Participant.Id}",
                Participant
            );

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Възникна грешка при редактирането на участника."
                );

                return Page();
            }

            return RedirectToPage("/Admin/Participants/Index");
        }
    }
}