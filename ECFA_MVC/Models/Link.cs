using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class Link
{
    public int NtId { get; set; }

    public string NtTitle { get; set; } = null!;

    public string Url { get; set; } = null!;

    public string ImgPath { get; set; } = null!;

    public string Target { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int Sort { get; set; }

    public bool Publish { get; set; }

    public DateTime? UpdDate { get; set; }

    public string? UpdUser { get; set; }
}
