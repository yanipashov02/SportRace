using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportRace.Infrastructure.Data;

namespace SportRace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly SportRaceDbContext _context;

    public StatisticsController(SportRaceDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetStatistics([FromQuery] int? eventId = null)
    {
        var eventsCount = await _context.Events.CountAsync();
        var participantsCount = await _context.Participants.CountAsync();
        var registrationsCount = await _context.Registrations.CountAsync();
        var resultsCount = await _context.Results.CountAsync();
        var finishedCount = await _context.Results.CountAsync(x => x.IsFinished);

        var eventReports = await _context.Events
            .AsNoTracking()
            .Where(x => !eventId.HasValue || x.Id == eventId.Value)
            .OrderByDescending(x => x.EventDate)
            .Select(x => new
            {
                x.Id,
                x.Name,
                x.Location,
                x.EventDate,
                RegistrationsCount = x.Registrations.Count,
                ResultsCount = _context.Results.Count(r => r.Registration.EventId == x.Id),
                FinishedCount = _context.Results.Count(r => r.Registration.EventId == x.Id && r.IsFinished),
                BestFinalTime = _context.Results
                    .Where(r => r.Registration.EventId == x.Id && r.IsFinished)
                    .OrderBy(r => r.FinalTime)
                    .Select(r => (TimeSpan?)r.FinalTime)
                    .FirstOrDefault()
            })
            .ToListAsync();

        return Ok(new
        {
            EventsCount = eventsCount,
            ParticipantsCount = participantsCount,
            RegistrationsCount = registrationsCount,
            ResultsCount = resultsCount,
            FinishedCount = finishedCount,
            EventReports = eventReports
        });
    }
}
