using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportRace.Domain.Entities;
using System.Net.Http.Json;

namespace SportRace.Web.Pages.Admin.Disciplines
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var client =
                _httpClientFactory.CreateClient("SportRaceApi1");

            var response =
                await client.PostAsJsonAsync(
                    "api/disciplines",
                    Discipline);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Възникна грешка при създаването на дисциплината.");

                return Page();
            }

            return RedirectToPage("/Admin/Disciplines/Index");
        }
    }
}