using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportRace.Domain.Entities;
using SportRace.Infrastructure.Data;

namespace SportRace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParticipantsController : ControllerBase
    {
        private readonly SportRaceDbContext _context;

        public ParticipantsController(SportRaceDbContext context)
        {
            _context = context;
        }

        // GET: api/participants
        [HttpGet]
        public async Task<IActionResult> GetParticipants()
        {
            var participants = await _context.Participants
                .Select(p => new
                {
                    p.Id,
                    p.FirstName,
                    p.LastName,
                    p.Email,
                    p.DateOfBirth,
                    p.PhoneNumber,
                    p.EmergencyContactName,
                    p.EmergencyContactPhone
                })
                .ToListAsync();

            return Ok(participants);
        }

        // POST: api/participants
        [HttpPost]
        public async Task<IActionResult> CreateParticipant(Participant participant)
        {
            _context.Participants.Add(participant);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetParticipants),
                new { id = participant.Id },
                participant
            );
        }

        // PUT: api/participants/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateParticipant(
            int id,
            Participant participant)
        {
            if (id != participant.Id)
            {
                return BadRequest();
            }

            var existingParticipant =
                await _context.Participants.FindAsync(id);

            if (existingParticipant == null)
            {
                return NotFound();
            }

            existingParticipant.FirstName = participant.FirstName;
            existingParticipant.LastName = participant.LastName;
            existingParticipant.Email = participant.Email;
            existingParticipant.DateOfBirth = participant.DateOfBirth;
            existingParticipant.PhoneNumber = participant.PhoneNumber;
            existingParticipant.EmergencyContactName =
                participant.EmergencyContactName;
            existingParticipant.EmergencyContactPhone =
                participant.EmergencyContactPhone;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/participants/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParticipant(int id)
        {
            var participant =
                await _context.Participants.FindAsync(id);

            if (participant == null)
            {
                return NotFound();
            }

            _context.Participants.Remove(participant);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}