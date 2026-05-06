using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Nodes;

namespace Dinamiques.Pages;

public class HabitatgesModel : PageModel
{
    private readonly ILogger<HabitatgesModel> _logger;

    public JsonArray LlistaHabitatges { get; set; }

    public HabitatgesModel(ILogger<HabitatgesModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGetAsync(string cp)
    {
        using var http = new HttpClient();
        if(cp == null)
        {

            var json1 = await http.GetStringAsync("http://localhost:5238/llistadhabitatges");
            LlistaHabitatges = JsonNode.Parse(json1)?.AsArray();
            return;
        }
        
        var json = await http.GetStringAsync("http://localhost:5238/PoblacioLlistaHabitatges/"+cp);
        LlistaHabitatges = JsonNode.Parse(json)?.AsArray();
    }
}