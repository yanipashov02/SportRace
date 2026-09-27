using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportRace.Domain.Entities;
using SportRace.Infrastructure.Data;

namespace SportRace.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DisciplinesController : ControllerBase
    {
        private readonly SportRaceDbContext _context;

        public DisciplinesController(SportRaceDbContext context)
        {
            _context = context;
        }

        // GET: api/disciplines
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Discipline>>> GetDisciplines()
        {
            var disciplines = await _context.Disciplines
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name)
                .ToListAsync();

            return Ok(disciplines);
        }

        // GET: api/disciplines/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Discipline>> GetDiscipline(int id)
        {
            var discipline = await _context.Disciplines
                .FirstOrDefaultAsync(d => d.Id == id);

            if (discipline == null)
            {
                return NotFound();
            }

            return Ok(discipline);
        }

        // POST: api/disciplines
        [HttpPost]
        public async Task<ActionResult<Discipline>> CreateDiscipline(
            Discipline discipline)
        {
            _context.Disciplines.Add(discipline);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetDiscipline),
                new { id = discipline.Id },
                discipline
            );
        }

        // PUT: api/disciplines/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscipline(
            int id,
            Discipline discipline)
        {
            var existingDiscipline =
                await _context.Disciplines.FindAsync(id);

            if (existingDiscipline == null)
            {
                return NotFound();
            }

            existingDiscipline.Name = discipline.Name;
            existingDiscipline.Description = discipline.Description;
            existingDiscipline.IsActive = discipline.IsActive;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/disciplines/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscipline(int id)
        {
            var discipline =
                await _context.Disciplines.FindAsync(id);

            if (discipline == null)
            {
                return NotFound();
            }

            _context.Disciplines.Remove(discipline);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}