using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportRace.Domain.Entities;
using SportRace.Infrastructure.Data;

namespace SportRace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StartListsController : ControllerBase
{
    private readonly SportRaceDbContext _context;
    public StartListsController(SportRaceDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetStartLists()
    {
        var lists = await _context.StartLists.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new { x.Id, x.EventId, EventName = _context.Events.Where(e => e.Id == x.EventId).Select(e => e.Name).FirstOrDefault(), x.CreatedAt, x.IsPublished, EntryCount = x.Entries.Count })
            .ToListAsync();
        return Ok(lists);
    }

    [HttpGet("{id}/entries")]
    public async Task<IActionResult> GetEntries(int id)
    {
        var entries = await _context.StartListEntries.AsNoTracking().Where(x => x.StartListId == id)
            .OrderBy(x => x.ScheduledStartTime).ThenBy(x => x.StartNumber)
            .Select(x => new { x.Id, x.StartListId, x.RegistrationId, x.StartNumber, x.ScheduledStartTime,
                ParticipantName = x.Registration.Participant.FirstName + " " + x.Registration.Participant.LastName,
                DisciplineName = x.Registration.Discipline.Name, CategoryName = x.Registration.Category.Name })
            .ToListAsync();
        return Ok(entries);
    }

    [HttpPost("generate/{eventId}")]
    public async Task<IActionResult> Generate(int eventId)
    {
        if (!await _context.Events.AnyAsync(x => x.Id == eventId)) return NotFound("Event not found.");
        var registrations = await _context.Registrations.AsNoTracking()
            .Where(x => x.EventId == eventId && x.Status != "Rejected").OrderBy(x => x.Id).ToListAsync();
        if (registrations.Count == 0) return BadRequest("There are no registrations for this event.");

        var list = new StartList { EventId = eventId, CreatedAt = DateTime.UtcNow, IsPublished = false };
        var start = DateTime.UtcNow;
        for (var i = 0; i < registrations.Count; i++)
            list.Entries.Add(new StartListEntry { RegistrationId = registrations[i].Id, StartNumber = i + 1, ScheduledStartTime = start.AddMinutes(i * 5) });
        _context.StartLists.Add(list);
        await _context.SaveChangesAsync();
        return Ok(new { list.Id });
    }

    [HttpPut("{id}/publish")]
    public async Task<IActionResult> Publish(int id)
    {
        var list = await _context.StartLists.FindAsync(id);
        if (list is null) return NotFound();
        list.IsPublished = true;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var list = await _context.StartLists.Include(x => x.Entries).FirstOrDefaultAsync(x => x.Id == id);
        if (list is null) return NotFound();
        _context.StartListEntries.RemoveRange(list.Entries);
        _context.StartLists.Remove(list);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
