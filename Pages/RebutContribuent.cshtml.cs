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
    public double QuotaGeneral = 0;

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

            double quota_subtotal= 0;
            double quota_total =0;
            int m2DeLaPropietat = int.Parse(habitatgesContribuent[i]["metresQuadrats"].ToString());

            if (habitatgesContribuent[i]["tipusImmoble"].ToString() == "0" )// tipusImmoble[0])
            {
                
                Num_casesTot++;
                M2casesTot += m2DeLaPropietat;
                quota_subtotal= 0.998*m2DeLaPropietat;
                double descompteFamiliaNumerosa =0;
                double incrementMenors =0;

                if (int.Parse(habitatgesContribuent[i]["habitantsImmoble"].ToString())>=5 )
                {
                     descompteFamiliaNumerosa = quota_subtotal*0.1;
                }
                 if (int.Parse(habitatgesContribuent[i]["numMenorsImmoble"].ToString())>0 )
                {
                     incrementMenors = quota_subtotal*0.05;
                }

                quota_total = quota_subtotal -descompteFamiliaNumerosa + incrementMenors;

            }

            if (habitatgesContribuent[i]["tipusImmoble"].ToString() == "1")
            {
                Num_pisosTot++;
                M2pisosTot += m2DeLaPropietat;
                quota_subtotal= 0.996*m2DeLaPropietat;
                double descompteFamiliaNumerosa =0;
                double incrementMenors =0;

                if (int.Parse(habitatgesContribuent[i]["habitantsImmoble"].ToString())>=5 )
                {
                     descompteFamiliaNumerosa = quota_subtotal*0.1;
                }
                 if (int.Parse(habitatgesContribuent[i]["numMenorsImmoble"].ToString())>0 )
                {
                     incrementMenors = quota_subtotal*0.05;
                }

                quota_total = quota_subtotal -descompteFamiliaNumerosa + incrementMenors;

            }

            if (habitatgesContribuent[i]["tipusImmoble"].ToString() == "2")
            {
                Num_terrenysTot++;
                M2terrenysTot += m2DeLaPropietat;
                quota_subtotal= 0.996*m2DeLaPropietat;
            }
            if (habitatgesContribuent[i]["codiPostal"].ToString() == "17970" || habitatgesContribuent[i]["codiPostal"].ToString() == "17971" )
            {
                quota_total = quota_total-quota_subtotal*0.25;
            }
            QuotaGeneral += quota_total;
        }



    }
}



