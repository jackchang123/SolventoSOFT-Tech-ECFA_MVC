using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class Atsfaq
{
    public int NtId { get; set; }

    public int NtParentId { get; set; }

    public string NtTitle { get; set; } = null!;

    public string NtContent { get; set; } = null!;

    public string? NtRelated { get; set; }

    public DateTime? NtCrtDate { get; set; }

    public string? NtCrtUser { get; set; }

    public DateTime? NtModDate { get; set; }

    public string? NtModUser { get; set; }
}
