using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportRace.Domain.Entities;
using SportRace.Infrastructure.Data;

namespace SportRace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResultsController : ControllerBase
{
    private readonly SportRaceDbContext _context;

    public ResultsController(SportRaceDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetResults([FromQuery] int? eventId = null)
    {
        var query = _context.Results
            .AsNoTracking()
            .AsQueryable();

        if (eventId.HasValue)
        {
            query = query.Where(x =>
                x.Registration.EventId == eventId.Value);
        }

        var results = await query
            .OrderBy(x => x.Registration.EventId)
            .ThenBy(x => x.Position)
            .ThenBy(x => x.FinalTime)
            .Select(x => new
            {
                x.Id,
                x.RegistrationId,

                ParticipantName =
                    x.Registration.Participant.FirstName + " " +
                    x.Registration.Participant.LastName,

                EventName = x.Registration.Event.Name,
                DisciplineName = x.Registration.Discipline.Name,
                CategoryName = x.Registration.Category.Name,
                AgeGroupName = x.Registration.AgeGroup.Name,

                x.TotalTime,
                x.PenaltySeconds,
                x.FinalTime,
                x.Position,
                x.IsFinished,
                x.RecordedAt
            })
            .ToListAsync();

        return Ok(results);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Result>> GetResult(int id)
    {
        var result = await _context.Results
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (result is null)
        {
            return NotFound();
        }

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> CreateResult(CreateResultRequest request)
    {
        var registrationExists =
            await _context.Registrations
                .AnyAsync(x => x.Id == request.RegistrationId);

        if (!registrationExists)
        {
            return BadRequest("Invalid registration.");
        }

        var resultExists =
            await _context.Results
                .AnyAsync(x =>
                    x.RegistrationId == request.RegistrationId);

        if (resultExists)
        {
            return Conflict(
                "A result for this registration already exists.");
        }

        if (request.TotalTime < TimeSpan.Zero)
        {
            return BadRequest("Total time cannot be negative.");
        }

        if (request.PenaltySeconds < 0)
        {
            return BadRequest("Penalty seconds cannot be negative.");
        }

        var result = new Result
        {
            RegistrationId = request.RegistrationId,
            TotalTime = request.TotalTime,
            PenaltySeconds = request.PenaltySeconds,
            FinalTime = request.TotalTime.Add(
                TimeSpan.FromSeconds(request.PenaltySeconds)),
            IsFinished = request.IsFinished,
            Position = 0,
            RecordedAt = DateTime.UtcNow
        };

        _context.Results.Add(result);

        await _context.SaveChangesAsync();

        await RecalculateRanking(result.RegistrationId);

        return CreatedAtAction(
            nameof(GetResult),
            new { id = result.Id },
            result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateResult(
        int id,
        UpdateResultRequest request)
    {
        var result = await _context.Results.FindAsync(id);

        if (result is null)
        {
            return NotFound();
        }

        if (request.TotalTime < TimeSpan.Zero)
        {
            return BadRequest("Total time cannot be negative.");
        }

        if (request.PenaltySeconds < 0)
        {
            return BadRequest("Penalty seconds cannot be negative.");
        }

        result.TotalTime = request.TotalTime;
        result.PenaltySeconds = request.PenaltySeconds;
        result.IsFinished = request.IsFinished;

        result.FinalTime = request.TotalTime.Add(
            TimeSpan.FromSeconds(request.PenaltySeconds));

        result.RecordedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        await RecalculateRanking(result.RegistrationId);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteResult(int id)
    {
        var result = await _context.Results.FindAsync(id);

        if (result is null)
        {
            return NotFound();
        }

        var registrationId = result.RegistrationId;

        _context.Results.Remove(result);

        await _context.SaveChangesAsync();

        await RecalculateRanking(registrationId);

        return NoContent();
    }

    private async Task RecalculateRanking(int registrationId)
    {
        var registration =
            await _context.Registrations
                .AsNoTracking()
                .FirstAsync(x => x.Id == registrationId);

        var results = await _context.Results
            .Where(x =>
                x.Registration.EventId == registration.EventId &&
                x.Registration.DisciplineId == registration.DisciplineId &&
                x.Registration.CategoryId == registration.CategoryId &&
                x.Registration.AgeGroupId == registration.AgeGroupId)
            .OrderByDescending(x => x.IsFinished)
            .ThenBy(x => x.FinalTime)
            .ToListAsync();

        var position = 1;

        foreach (var item in results)
        {
            item.Position =
                item.IsFinished ? position++ : 0;
        }

        await _context.SaveChangesAsync();
    }

    public class CreateResultRequest
    {
        public int RegistrationId { get; set; }

        public TimeSpan TotalTime { get; set; }

        public int PenaltySeconds { get; set; }

        public bool IsFinished { get; set; }
    }

    public class UpdateResultRequest
    {
        public TimeSpan TotalTime { get; set; }

        public int PenaltySeconds { get; set; }

        public bool IsFinished { get; set; }
    }
}