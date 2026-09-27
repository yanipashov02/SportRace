using Microsoft.AspNetCore.Mvc.RazorPages;
using SportRace.Domain.Entities;
using System.Net.Http.Json;

namespace SportRace.Web.Pages.Admin.Statistics;

public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _factory;

    public IndexModel(IHttpClientFactory factory) => _factory = factory;

    public StatisticsViewModel Statistics { get; set; } = new();
    public List<Event> Events { get; set; } = new();
    public int? EventId { get; set; }
    public string? ErrorMessage { get; set; }

    public async Task OnGetAsync(int? eventId)
    {
        EventId = eventId;
        var client = _factory.CreateClient("SportRaceApi1");

        try
        {
            Events = await client.GetFromJsonAsync<List<Event>>("api/events") ?? new();
            var url = eventId.HasValue ? $"api/statistics?eventId={eventId.Value}" : "api/statistics";
            Statistics = await client.GetFromJsonAsync<StatisticsViewModel>(url) ?? new();
        }
        catch (HttpRequestException)
        {
            ErrorMessage = "Статистиката не може да бъде заредена. Проверете дали SportRace.API е стартиран.";
        }
    }

    public class StatisticsViewModel
    {
        public int EventsCount { get; set; }
        public int ParticipantsCount { get; set; }
        public int RegistrationsCount { get; set; }
        public int ResultsCount { get; set; }
        public int FinishedCount { get; set; }
        public List<EventReportViewModel> EventReports { get; set; } = new();
    }

    public class EventReportViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public int RegistrationsCount { get; set; }
        public int ResultsCount { get; set; }
        public int FinishedCount { get; set; }
        public TimeSpan? BestFinalTime { get; set; }
    }
}
