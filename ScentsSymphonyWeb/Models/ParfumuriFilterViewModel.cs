using ScentsSymphonyWeb.Models;
using System.Collections.Generic;

public class ParfumuriFilterViewModel
{
    public List<Parfumuri> Parfumuri { get; set; }

    public string? Brand { get; set; }
    public decimal? PretMin { get; set; }
    public decimal? PretMax { get; set; }

    public List<string> Branduri { get; set; }

    public string? Nisa { get; set; }
    public List<string> NiseDisponibile { get; set; }

    public string? CantitateDisponibila { get; set; }
    public List<string> CantitatiDisponibile { get; set; }
}
