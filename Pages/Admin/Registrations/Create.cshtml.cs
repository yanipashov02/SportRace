using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http.Json;
using SportRace.Domain.Entities;

namespace SportRace.Web.Pages.Admin.Registrations
{
    public class CreateModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public CreateModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [BindProperty]
        public Registration Registration { get; set; } = new();

        public List<Participant> Participants { get; set; } = new();
        public List<Event> Events { get; set; } = new();
        public List<Discipline> Disciplines { get; set; } = new();
        public List<Category> Categories { get; set; } = new();
        public List<AgeGroup> AgeGroups { get; set; } = new();

        public async Task OnGetAsync()
        {
            await LoadDataAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Навигационните свойства не се попълват от HTML формата.
            // За създаването на регистрация са ни необходими техните ID-та.
            ModelState.Remove("Registration.Participant");
            ModelState.Remove("Registration.Event");
            ModelState.Remove("Registration.Discipline");
            ModelState.Remove("Registration.Category");
            ModelState.Remove("Registration.AgeGroup");

            if (Registration.ParticipantId <= 0)
            {
                ModelState.AddModelError(
                    "Registration.ParticipantId",
                    "Моля, изберете участник.");
            }

            if (Registration.EventId <= 0)
            {
                ModelState.AddModelError(
                    "Registration.EventId",
                    "Моля, изберете състезание.");
            }

            if (Registration.DisciplineId <= 0)
            {
                ModelState.AddModelError(
                    "Registration.DisciplineId",
                    "Моля, изберете дисциплина.");
            }

            if (Registration.CategoryId <= 0)
            {
                ModelState.AddModelError(
                    "Registration.CategoryId",
                    "Моля, изберете категория.");
            }

            if (Registration.AgeGroupId <= 0)
            {
                ModelState.AddModelError(
                    "Registration.AgeGroupId",
                    "Моля, изберете възрастова група.");
            }

            if (!ModelState.IsValid)
            {
                await LoadDataAsync();
                return Page();
            }

            var client =
                _httpClientFactory.CreateClient("SportRaceApi1");

            var response = await client.PostAsJsonAsync(
                "api/registrations",
                Registration);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent =
                    await response.Content.ReadAsStringAsync();

                ModelState.AddModelError(
                    string.Empty,
                    $"Възникна грешка при създаване на регистрацията. " +
                    $"Код: {(int)response.StatusCode}. {errorContent}");

                await LoadDataAsync();
                return Page();
            }

            return RedirectToPage("/Admin/Registrations/Index");
        }

        private async Task LoadDataAsync()
        {
            var client =
                _httpClientFactory.CreateClient("SportRaceApi1");

            Participants =
                await client.GetFromJsonAsync<List<Participant>>(
                    "api/participants") ?? new();

            Events =
                await client.GetFromJsonAsync<List<Event>>(
                    "api/events") ?? new();

            Disciplines =
                await client.GetFromJsonAsync<List<Discipline>>(
                    "api/disciplines") ?? new();

            Categories =
                await client.GetFromJsonAsync<List<Category>>(
                    "api/categories") ?? new();

            AgeGroups =
                await client.GetFromJsonAsync<List<AgeGroup>>(
                    "api/agegroups") ?? new();
        }
    }
}