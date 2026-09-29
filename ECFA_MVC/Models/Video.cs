using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class Video
{
    public int Ntid { get; set; }

    public string NtTitle { get; set; } = null!;

    public string NtUrl { get; set; } = null!;

    public string NtPubDate { get; set; } = null!;

    public bool NtBlock { get; set; }

    public int NtSort { get; set; }

    public DateTime? NtCrDate { get; set; }

    public string? NtCrUser { get; set; }

    public DateTime? NtModDate { get; set; }

    public string? NtModUser { get; set; }
}
