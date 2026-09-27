using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SportRace.Domain.Entities;
using SportRace.Infrastructure.Data;

namespace SportRace.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly SportRaceDbContext _context;
    private readonly PasswordHasher<User> _hasher = new();
    public UsersController(SportRaceDbContext context) => _context = context;

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);
        if (user is null || _hasher.VerifyHashedPassword(user, user.PasswordHash, request.Password) == PasswordVerificationResult.Failed)
            return Unauthorized();
        return Ok(new { user.Id, user.FirstName, user.LastName, user.Email, Role = user.Role.ToString() });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        if (await _context.Users.AnyAsync(x => x.Email == request.Email)) return Conflict("Email already exists.");
        var user = new User { FirstName = request.FirstName, LastName = request.LastName, Email = request.Email, Role = Domain.Enums.UserRole.Participant, CreatedAt = DateTime.UtcNow };
        user.PasswordHash = _hasher.HashPassword(user, request.Password);
        _context.Users.Add(user); await _context.SaveChangesAsync();
        return Ok(new { user.Id, user.Email });
    }

    public record LoginRequest(string Email, string Password);
    public record RegisterRequest(string FirstName, string LastName, string Email, string Password);
}
