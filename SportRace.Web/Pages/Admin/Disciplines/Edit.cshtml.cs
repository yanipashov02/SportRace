using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportRace.Domain.Entities;
using System.Net.Http.Json;

namespace SportRace.Web.Pages.Admin.Disciplines
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public Discipline Discipline { get; set; } = new();

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var discipline = await client.GetFromJsonAsync<Discipline>(
                $"api/disciplines/{id}");

            if (discipline == null)
            {
                return NotFound();
            }

            Discipline = discipline;

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
                $"api/disciplines/{Discipline.Id}",
                Discipline);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Възникна грешка при редактирането на дисциплината.");

                return Page();
            }

            return RedirectToPage("/Admin/Disciplines/Index");
        }
    }
}