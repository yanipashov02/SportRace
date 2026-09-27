using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportRace.Domain.Entities;
using System.Net.Http.Json;

namespace SportRace.Web.Pages.Admin.Categories
{
    public class EditModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public EditModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Category Category { get; set; } = new Category();

        public async Task<IActionResult> OnGetAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var response = await client.GetAsync($"api/categories/{id}");

            if (!response.IsSuccessStatusCode)
            {
                return NotFound();
            }

            Category = await response.Content.ReadFromJsonAsync<Category>();

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
                $"api/categories/{Category.Id}",
                Category);

            if (!response.IsSuccessStatusCode)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Възникна грешка при редактиране на категорията.");

                return Page();
            }

            return RedirectToPage("/Admin/Categories/Index");
        }
    }
}