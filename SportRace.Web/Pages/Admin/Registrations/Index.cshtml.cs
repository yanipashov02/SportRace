using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;

namespace SportRace.Web.Pages.Admin.Registrations
{
    public class IndexModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public List<RegistrationViewModel> Registrations { get; set; } = new();

        public IndexModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task OnGetAsync()
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var registrations =
                await client.GetFromJsonAsync<List<RegistrationViewModel>>(
                    "api/registrations");

            Registrations = registrations ?? new List<RegistrationViewModel>();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var client = _httpClientFactory.CreateClient("SportRaceApi1");

            var response =
                await client.DeleteAsync($"api/registrations/{id}");

            if (!response.IsSuccessStatusCode)
            {
                TempData["ErrorMessage"] =
                    "Възникна грешка при изтриването на регистрацията.";
            }

            return RedirectToPage();
        }

        public class RegistrationViewModel
        {
            public int Id { get; set; }

            public int ParticipantId { get; set; }

            public string ParticipantName { get; set; } = string.Empty;

            public int EventId { get; set; }

            public string EventName { get; set; } = string.Empty;

            public int DisciplineId { get; set; }

            public string DisciplineName { get; set; } = string.Empty;

            public int CategoryId { get; set; }

            public string CategoryName { get; set; } = string.Empty;

            public int AgeGroupId { get; set; }

            public string AgeGroupName { get; set; } = string.Empty;

            public DateTime RegisteredAt { get; set; }

            public string Status { get; set; } = string.Empty;
        }
    }
}