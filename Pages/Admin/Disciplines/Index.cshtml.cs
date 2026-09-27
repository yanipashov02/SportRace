using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportRace.Domain.Entities;
using System.Net.Http.Json;

namespace SportRace.Web.Pages.Admin.Disciplines
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<Discipline> Disciplines { get; set; } = new();

        [TempData]
        public string? SuccessMessage { get; set; }

        [TempData]
        public string? ErrorMessage { get; set; }

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var disciplines =
                await client.GetFromJsonAsync<List<Discipline>>("api/disciplines");

            Disciplines = disciplines ?? new List<Discipline>();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var response =
                await client.DeleteAsync($"api/disciplines/{id}");

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Дисциплината беше изтрита успешно.";
            }
            else
            {
                ErrorMessage = "Възникна грешка при изтриването на дисциплината.";
            }

            return RedirectToPage();
        }
    }
}