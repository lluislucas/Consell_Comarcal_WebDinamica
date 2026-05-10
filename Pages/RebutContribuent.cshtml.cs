using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json.Nodes;

namespace Dinamiques.Pages;

public class RebutContribuentModel : PageModel
{
    private readonly ILogger<RebutContribuentModel> _logger;
    public Dictionary<string, Dictionary<string, List<JsonNode>>> habitatgesXcp { get; set; } = new();

    public string Id { get; set; }

    public JsonNode HabitatgeSeleccionat { get; set; }

    public string Cp { get; set; }
    public string Dni { get; set; }

    public int Num_casesTot = 0;
    public int Num_pisosTot = 0;
    public int Num_terrenysTot = 0;
    public bool Descomptetassa = false;

    public int M2casesTot = 0;
    public int M2pisosTot = 0;
    public int M2terrenysTot = 0;
    public double Quotatotal = 0;

    public RebutContribuentModel(ILogger<RebutContribuentModel> logger)
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

        }

        List<JsonNode> habitatgesContribuent = habitatgesXcp[cp][dni];


        for (int i = 0; i < habitatgesContribuent.Count; i++)
        {
            Console.WriteLine(habitatgesContribuent[i]["metresQuadrats"].GetType());
            if (habitatgesContribuent[i]["tipusImmoble"].ToString() == "0" )// tipusImmoble[0])
            {
                int m2DeLaCasa = int.Parse(habitatgesContribuent[i]["metresQuadrats"].ToString());
                Num_casesTot++;
                M2casesTot += m2DeLaCasa;
                int quota_casa= 0.998*m2DeLaCasa;
                if(habitatgesContribuent[i]["habitantsImmoble"]>=5 )
                {
                    
                }
            }

            if (habitatgesContribuent[i]["tipusImmoble"].ToString() == "1")
            {
                Num_pisosTot++;
                M2pisosTot += int.Parse(habitatgesContribuent[i]["metresQuadrats"].ToString());
            }

            if (habitatgesContribuent[i]["tipusImmoble"].ToString() == "2")
            {
                Num_terrenysTot++;
                M2terrenysTot += int.Parse(habitatgesContribuent[i]["metresQuadrats"].ToString());
            }
        }



    }
}



