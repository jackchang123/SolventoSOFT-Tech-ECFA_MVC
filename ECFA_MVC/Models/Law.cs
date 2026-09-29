using System;
using System.Collections.Generic;

namespace ECFA_MVC.Models;

public partial class Law
{
    public int NtId { get; set; }

    public int NtTypeId { get; set; }

    public string NtTitle { get; set; } = null!;

    public byte[]? NtFile { get; set; }

    public string? NtFileName { get; set; }

    public string? NtKeyWord { get; set; }

    public string? NtAuthor { get; set; }

    public string? NtPubDate { get; set; }

    public DateTime? NtCrDate { get; set; }

    public string? NtCrUser { get; set; }

    public DateTime? NtModDate { get; set; }

    public string? NtModUser { get; set; }
}
