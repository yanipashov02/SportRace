using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportRace.Domain.Entities;
using SportRace.Infrastructure.Data;

namespace SportRace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationsController : ControllerBase
    {
        private readonly SportRaceDbContext _context;

        public RegistrationsController(SportRaceDbContext context)
        {
            _context = context;
        }

        // GET: api/registrations
        [HttpGet]
        public async Task<IActionResult> GetRegistrations()
        {
            var registrations = await _context.Registrations
                .Select(r => new
                {
                    r.Id,
                    r.ParticipantId,
                    ParticipantName = r.Participant.FirstName + " " +
                                      r.Participant.LastName,

                    r.EventId,
                    EventName = r.Event.Name,

                    r.DisciplineId,
                    DisciplineName = r.Discipline.Name,

                    r.CategoryId,
                    CategoryName = r.Category.Name,

                    r.AgeGroupId,
                    AgeGroupName = r.AgeGroup.Name,

                    r.RegisteredAt,
                    r.Status
                })
                .ToListAsync();

            return Ok(registrations);
        }

        // GET: api/registrations/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegistration(int id)
        {
            var registration = await _context.Registrations
                .Where(r => r.Id == id)
                .Select(r => new
                {
                    r.Id,
                    r.ParticipantId,
                    ParticipantName = r.Participant.FirstName + " " +
                                      r.Participant.LastName,

                    r.EventId,
                    EventName = r.Event.Name,

                    r.DisciplineId,
                    DisciplineName = r.Discipline.Name,

                    r.CategoryId,
                    CategoryName = r.Category.Name,

                    r.AgeGroupId,
                    AgeGroupName = r.AgeGroup.Name,

                    r.RegisteredAt,
                    r.Status
                })
                .FirstOrDefaultAsync();

            if (registration == null)
            {
                return NotFound();
            }

            return Ok(registration);
        }

        // POST: api/registrations
        [HttpPost]
        public async Task<IActionResult> CreateRegistration(
            Registration registration)
        {
            registration.RegisteredAt = DateTime.UtcNow;
            registration.Status = "Pending";

            _context.Registrations.Add(registration);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetRegistration),
                new { id = registration.Id },
                registration
            );
        }

        // PUT: api/registrations/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRegistration(
            int id,
            Registration registration)
        {
            if (id != registration.Id)
            {
                return BadRequest();
            }

            var existingRegistration =
                await _context.Registrations.FindAsync(id);

            if (existingRegistration == null)
            {
                return NotFound();
            }

            existingRegistration.ParticipantId =
                registration.ParticipantId;

            existingRegistration.EventId =
                registration.EventId;

            existingRegistration.DisciplineId =
                registration.DisciplineId;

            existingRegistration.CategoryId =
                registration.CategoryId;

            existingRegistration.AgeGroupId =
                registration.AgeGroupId;

            existingRegistration.Status =
                registration.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/registrations/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRegistration(int id)
        {
            var registration =
                await _context.Registrations.FindAsync(id);

            if (registration == null)
            {
                return NotFound();
            }

            _context.Registrations.Remove(registration);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}