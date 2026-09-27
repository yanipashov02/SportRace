using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportRace.Domain.Entities;
using System.Net.Http.Json;

namespace SportRace.Web.Pages.Admin.Categories
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public Category Category { get; set; } = new Category();

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

            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var response = await client.PostAsJsonAsync(
                "api/categories",
                Category);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Възникна грешка при създаването на категорията.");

                return Page();
            }

            return RedirectToPage("/Admin/Categories/Index");
        }
    }
}