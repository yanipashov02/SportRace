using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportRace.Domain.Entities;
using SportRace.Infrastructure.Data;

namespace SportRace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgeGroupsController : ControllerBase
    {
        private readonly SportRaceDbContext _context;

        public AgeGroupsController(SportRaceDbContext context)
        {
            _context = context;
        }

        // GET: api/agegroups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AgeGroup>>> GetAgeGroups()
        {
            var ageGroups = await _context.AgeGroups
                .Where(a => a.IsActive)
                .OrderBy(a => a.MinAge)
                .ToListAsync();

            return Ok(ageGroups);
        }

        // GET: api/agegroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AgeGroup>> GetAgeGroup(int id)
        {
            var ageGroup = await _context.AgeGroups
                .FirstOrDefaultAsync(a => a.Id == id);

            if (ageGroup == null)
            {
                return NotFound();
            }

            return Ok(ageGroup);
        }

        // POST: api/agegroups
        [HttpPost]
        public async Task<ActionResult<AgeGroup>> CreateAgeGroup(
            AgeGroup ageGroup)
        {
            _context.AgeGroups.Add(ageGroup);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetAgeGroup),
                new { id = ageGroup.Id },
                ageGroup
            );
        }

        // PUT: api/agegroups/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAgeGroup(
            int id,
            AgeGroup ageGroup)
        {
            var existingAgeGroup =
                await _context.AgeGroups.FindAsync(id);

            if (existingAgeGroup == null)
            {
                return NotFound();
            }

            existingAgeGroup.Name = ageGroup.Name;
            existingAgeGroup.MinAge = ageGroup.MinAge;
            existingAgeGroup.MaxAge = ageGroup.MaxAge;
            existingAgeGroup.IsActive = ageGroup.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/agegroups/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgeGroup(int id)
        {
            var ageGroup =
                await _context.AgeGroups.FindAsync(id);

            if (ageGroup == null)
            {
                return NotFound();
            }

            _context.AgeGroups.Remove(ageGroup);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}