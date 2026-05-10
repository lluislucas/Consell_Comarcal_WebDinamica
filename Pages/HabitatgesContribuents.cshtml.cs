using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Nodes;

namespace Dinamiques.Pages;

public class HabitatgesContribuentModel : PageModel
{
    private readonly ILogger<HabitatgesContribuentModel> _logger;
    public Dictionary<string, Dictionary<string, List<JsonNode>>> habitatgesXcp { get; set; } = new();

    public string Cp { get; set; }
    public string Dni { get; set; }

    public HabitatgesContribuentModel(ILogger<HabitatgesContribuentModel> logger)
    {
        _logger = logger;
    }

    public async Task OnGetAsync(string dni, string cp)
    {
        

        Cp = cp;
        Dni = dni;
        using var http = new HttpClient();
        var json1 = await http.GetStringAsync("http://localhost:5238/llistadhabitatges");


        JsonArray llistaHabitatges = JsonNode.Parse(json1)?.AsArray();

        for (int i = 0; i < llistaHabitatges.Count; i++)
        {
            string cparevisar = llistaHabitatges[i]["codiPostal"].ToString();

            if (cp == cparevisar || cp == null)
            {

                string dniContribuentArevisar = llistaHabitatges[i]["dniContribuent"].ToString();

                if (!habitatgesXcp.ContainsKey(cparevisar))
                {
                    habitatgesXcp[cparevisar] = new();
                }

                if (!habitatgesXcp[cparevisar].ContainsKey(dniContribuentArevisar))
                {
                    habitatgesXcp[cparevisar][dniContribuentArevisar] = new();
                }
                habitatgesXcp[cparevisar][dniContribuentArevisar].Add(llistaHabitatges[i]);
            }

        }//hem creat el diccionari



    }
}
