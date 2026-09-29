using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class PrgMenu201110
{
    public int NodeId { get; set; }

    public int ParentId { get; set; }

    public int Layer { get; set; }

    public int Sort { get; set; }

    public string PrgId { get; set; } = null!;

    public string NodeName { get; set; } = null!;

    public string? PrgPath { get; set; }

    public string? PrgTarget { get; set; }

    public string? IconPath { get; set; }

    public string AuthCode { get; set; } = null!;

    public DateTime? SetDate { get; set; }

    public string? SetUser { get; set; }
}
