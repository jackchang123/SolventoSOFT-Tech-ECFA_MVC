using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class Hdmad
{
    public int NtId { get; set; }

    public string NtTitle { get; set; } = null!;

    public byte[] NtFile { get; set; } = null!;

    public string NtFileName { get; set; } = null!;

    public string NtFileSize { get; set; } = null!;

    public byte[] NtFile2 { get; set; } = null!;

    public string NtFileName2 { get; set; } = null!;

    public string NtFileSize2 { get; set; } = null!;

    public int Sort { get; set; }

    public int NtParentId { get; set; }

    public string? NtModHistory { get; set; }
}
