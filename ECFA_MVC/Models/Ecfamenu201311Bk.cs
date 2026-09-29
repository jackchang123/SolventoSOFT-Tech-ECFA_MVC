using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class Ecfamenu201311Bk
{
    public int NodeId { get; set; }

    public int ParentId { get; set; }

    public int Layer { get; set; }

    public int Sort { get; set; }

    public string NodeName { get; set; } = null!;

    public string? PrgPath { get; set; }

    public string? PrgTarget { get; set; }

    public string? IconPath { get; set; }

    public DateTime? SetDate { get; set; }

    public string? SetUser { get; set; }
}
