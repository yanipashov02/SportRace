using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SportRace.Domain.Entities;
using System.Net.Http.Json;
namespace SportRace.Web.Pages.Admin.AgeGroups;
public class IndexModel : PageModel
{
    private readonly IHttpClientFactory _factory;
    public List<AgeGroup> AgeGroups { get; set; } = new();
    [TempData] public string? Message { get; set; }
    public IndexModel(IHttpClientFactory factory) => _factory = factory;
    public async Task OnGetAsync() => AgeGroups = await _factory.CreateClient("SportRaceApi1").GetFromJsonAsync<List<AgeGroup>>("api/agegroups") ?? new();
    public async Task<IActionResult> OnPostDeleteAsync(int id) { var r = await _factory.CreateClient("SportRaceApi1").DeleteAsync($"api/agegroups/{id}"); Message = r.IsSuccessStatusCode ? "Възрастовата група беше изтрита." : "Групата не може да бъде изтрита."; return RedirectToPage(); }
}
