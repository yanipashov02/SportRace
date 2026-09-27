using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportRace.Domain.Entities;
using SportRace.Infrastructure.Data;

namespace SportRace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly SportRaceDbContext _context;

        public EventsController(SportRaceDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _context.Events
                .AsNoTracking()
                .ToListAsync();

            return Ok(events);
        }
        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event newEvent)
        {
            newEvent.CreatedAt = DateTime.UtcNow;
            newEvent.IsPublished = false;

            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEvents), new { id = newEvent.Id }, newEvent);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event updatedEvent)
        {
            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null)
            {
                return NotFound();
            }

            existingEvent.Name = updatedEvent.Name;
            existingEvent.Description = updatedEvent.Description;
            existingEvent.Location = updatedEvent.Location;
            existingEvent.EventDate = updatedEvent.EventDate;
            existingEvent.RegistrationDeadline = updatedEvent.RegistrationDeadline;
            existingEvent.IsPublished = updatedEvent.IsPublished;

            await _context.SaveChangesAsync();

            return Ok(existingEvent);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null)
            {
                return NotFound();
            }

            _context.Events.Remove(existingEvent);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}