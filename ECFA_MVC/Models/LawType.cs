using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class LawType
{
    public int NtId { get; set; }

    public int? NtNodeId { get; set; }

    public string NtTypeName { get; set; } = null!;

    public DateTime? NtCrDate { get; set; }

    public string? NtCrUser { get; set; }

    public DateTime? NtModDate { get; set; }

    public string? NtModUser { get; set; }
}
