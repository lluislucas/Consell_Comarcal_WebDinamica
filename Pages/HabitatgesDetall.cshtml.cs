using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Nodes;

namespace Dinamiques.Pages;

public class HabitatgesDetallModel : PageModel
{
    private readonly ILogger<HabitatgesDetallModel> _logger;

    public string Id {get; set;}

    public JsonNode HabitatgeSeleccionat {get; set;}

    public HabitatgesDetallModel(ILogger<HabitatgesDetallModel> logger)
    {
        _logger = logger;
    }


    public async Task OnGetAsync(string id)
    {
        Id = id;

        using var http = new HttpClient();
        var json1 = await http.GetStringAsync("http://localhost:5238/llistadhabitatges");
        var LlistaHabitatges = JsonNode.Parse(json1)?.AsArray();

        for(int i =0; i< LlistaHabitatges.Count; i++)
        {
            if(Id == LlistaHabitatges[i]["identificador"]?.ToString())
            {
                HabitatgeSeleccionat = LlistaHabitatges[i];
                break;
            }

        }

    }
}