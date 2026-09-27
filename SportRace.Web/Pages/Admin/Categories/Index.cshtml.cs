using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportRace.Domain.Entities;
using System.Net.Http.Json;

namespace SportRace.Web.Pages.Admin.Categories
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<Category> Categories { get; set; } = new();

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

            var categories =
                await client.GetFromJsonAsync<List<Category>>("api/categories");

            Categories = categories ?? new List<Category>();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var response =
                await client.DeleteAsync($"api/categories/{id}");

            if (response.IsSuccessStatusCode)
            {
                SuccessMessage = "Категорията беше изтрита успешно.";
            }
            else
            {
                ErrorMessage = "Възникна грешка при изтриването на категорията.";
            }

            return RedirectToPage("/Admin/Categories/Index");
        }
    }
}